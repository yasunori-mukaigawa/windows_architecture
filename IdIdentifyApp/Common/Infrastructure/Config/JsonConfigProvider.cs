using IdIdentifyApp.Common.Contracts.Config;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Common.Infrastructure.Config;

/**
 * JSON ファイルから設定を読み込む標準実装。
 *
 * 本クラスは、指定された JSON ファイルを読み込み、
 * 指定型へデシリアライズして返す。
 *
 * ■ 提供する責務
 *   JSONファイルの存在確認
 *   設定ファイル格納ディレクトリの作成
 *   設定ファイルが存在しない場合のデフォルト生成
 *   ファイル読込
 *   JSONデシリアライズ
 *
 * ■ 設計上の意図
 *   Config 読込の仕組みを共通化し、
 *   各機能が個別に JSON 解析処理を持たないようにする。
 *
 * ■ 注意
 *   本クラスは「どう読むか」の責務のみを持つ。
 *   設定値が業務的に妥当かどうかの判断は行わない。
 *
 * ■ 補足
 *   defaultFactory を省略した場合は、
 *   Activator.CreateInstance<TConfig>() により既定インスタンスを生成する。
 *   そのため、規約ベース自動登録を利用する設定型は
 *   public parameterless constructor を持つことを前提とする。
 */
public sealed class JsonConfigProvider<TConfig> : IConfigProvider<TConfig>
    where TConfig : class
{
    // 読込対象の設定ファイルパス
    private readonly string _filePath;

    // 設定ファイルが存在しない場合に使用するデフォルト値生成処理
    private readonly Func<TConfig> _defaultFactory;

    // JSON デシリアライズ時の設定
    private readonly JsonSerializerOptions _serializerOptions;

    public JsonConfigProvider(
        string filePath,
        Func<TConfig>? defaultFactory = null,
        JsonSerializerOptions? serializerOptions = null)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("Config file path must not be empty.", nameof(filePath));
        }

        _filePath = filePath;
        _defaultFactory = defaultFactory ?? CreateDefaultInstance;
        _serializerOptions = serializerOptions ?? new JsonSerializerOptions
        {
            // JSON 側の大文字小文字差異を吸収する
            PropertyNameCaseInsensitive = true,

            // 自動生成時の JSON を読みやすくする
            WriteIndented = true
        };
    }

    /**
     * 設定を同期的に読み込む。
     *
     * 設定ファイルが存在しない場合は、
     * デフォルト値を生成してファイルを作成したうえで返す。
     *
     * 起動時の基盤初期化など、同期的に値を確定したい場面で使用する。
     */
    public TConfig Load()
    {
        // 設定ファイル格納先ディレクトリが存在しなければ作成する
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // 設定ファイルが存在しない場合はデフォルト値で新規作成する
        if (!File.Exists(_filePath))
        {
            var defaultConfig = _defaultFactory();

            var json = JsonSerializer.Serialize(defaultConfig, _serializerOptions);
            File.WriteAllText(_filePath, json);

            return defaultConfig;
        }

        var jsonText = File.ReadAllText(_filePath);

        // JSON を指定型へ変換する
        var config = JsonSerializer.Deserialize<TConfig>(jsonText, _serializerOptions);

        if (config is null)
        {
            throw new InvalidOperationException(
                $"Failed to deserialize config file to type {typeof(TConfig).FullName}.");
        }

        return config;
    }

    /**
     * 設定を非同期で読み込む。
     *
     * 設定ファイルが存在しない場合は、
     * デフォルト値を生成してファイルを作成したうえで返す。
     */
    public async Task<TConfig> LoadAsync(CancellationToken cancellationToken = default)
    {
        // 設定ファイル格納先ディレクトリが存在しなければ作成する
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // 設定ファイルが存在しない場合はデフォルト値で新規作成する
        if (!File.Exists(_filePath))
        {
            var defaultConfig = _defaultFactory();

            await using (var createStream = File.Create(_filePath))
            {
                await JsonSerializer.SerializeAsync(
                    createStream,
                    defaultConfig,
                    _serializerOptions,
                    cancellationToken);
            }

            return defaultConfig;
        }

        await using var readStream = File.OpenRead(_filePath);

        // JSON を指定型へ変換する
        var config = await JsonSerializer.DeserializeAsync<TConfig>(
            readStream,
            _serializerOptions,
            cancellationToken);

        if (config is null)
        {
            throw new InvalidOperationException(
                $"Failed to deserialize config file to type {typeof(TConfig).FullName}.");
        }

        return config;
    }

    /**
     * 既定インスタンスを生成する。
     *
     * defaultFactory が指定されない場合に使用する。
     */
    private static TConfig CreateDefaultInstance()
    {
        return Activator.CreateInstance<TConfig>()
            ?? throw new InvalidOperationException(
                $"Failed to create default instance for type {typeof(TConfig).FullName}.");
    }
}