using System.IO;
using UnityEngine;
using UnityEditor;


namespace UTJ.AssetBundleBuilder
{
	/// <summary>
	/// 取り合えずAssetBundleをビルドする為のシンプルなWindow
	/// </summary>
	public class AssetBundleBuilderEditorWindow : EditorWindow
	{
		static class Styles
		{
			public static readonly GUIContent BuildTarget = new GUIContent("Build Target", "AssetBundleのプラットフォームを選択します。");
			public static readonly GUIContent OutputPath = new GUIContent("Output Path", "AssetBundleの出力先のディレクトリ");
			public static readonly GUIContent BuildOptions = new GUIContent("Build Options", "AssetBundleビルド時のオプション");
			public static readonly GUIContent UncompressedAssetBundle = new GUIContent("Uncompressed AssetBundle", "AssetBundleを圧縮しない");
			public static readonly GUIContent DisableWriteTypeTree = new GUIContent("Disable Write TypeTree", "アセットバンドル内にタイプに関する情報を入れないようにします。");
			public static readonly GUIContent DeterministicAssetBundle = new GUIContent("Deterministic AssetBundle", "アセットバンドルに保管されているオブジェクト ID のハッシュを使用して、アセットバンドルを作成します。");
			public static readonly GUIContent ForceRebuildAssetBundle = new GUIContent("ForceRebuildAssetBundle", "強制的にアセットバンドルを再ビルドします。");
			public static readonly GUIContent IgnoreTypeTreeChanges = new GUIContent("IgnoreTypeTreeChanges", "インクリメンタルビルドチェックを行う場合、タイプツリーの変更を無視します。");
			public static readonly GUIContent AppendHashToAssetBundleName = new GUIContent("AppendHashToAssetBundleName", "アセットバンドル名にハッシュを追加します。");
			public static readonly GUIContent ChunkBasedCompression = new GUIContent("ChunkBasedCompression", "アセットバンドル作成時にチャンクベースの LZ4 圧縮を使用します。");
			public static readonly GUIContent StrictMode = new GUIContent("StrictMode", "ビルド中にエラーが発生したら、ビルドを中断します。");
			public static readonly GUIContent DryRunBuild = new GUIContent("DryRunBuild", "	Do a dry run build.");
			public static readonly GUIContent DisableLoadAssetByFileName = new GUIContent("DisableLoadAssetByFileName", "Disables Asset Bundle LoadAsset by file name.");
			public static readonly GUIContent DisableLoadAssetByFileNameWithExtension = new GUIContent("DisableLoadAssetByFileNameWithExtension ", "Removes the Unity Version number in the Archive File & Serialized File headers during the build.");
			public static readonly GUIContent AssetBundleStripUnityVersion = new GUIContent("AssetBundleStripUnityVersion", "Disables Asset Bundle LoadAsset by file name with extension.");
			public static readonly GUIContent Build = new GUIContent("Build", "Build AssetBundles");
			public static readonly GUIContent Copy = new GUIContent("Copy", " Copy AssetBundles from AssetBundle to StreamingAssets");
			public static readonly GUIContent Clear = new GUIContent("Clear", "Claer AssetBundles in StreamingAssets");
			public static readonly GUIContent CompressMode = new GUIContent("圧縮方法","AssetBundleの圧縮形式には非圧縮・LZMA・LZ4の3種類の中選択から選択可能です。非圧縮の場合、ファイルサイズは大きくなる為、ダウンロード時間、ディスクスペースの使用量は最も大きくなりますが、ロード時間は最短になります。LZMAはファイルサイズは最も小さくなりますが、ロード時にはAssetBunde全体を一括して解凍する必要がある為、ロード時間は最も遅くなります。LZ4はLZMAと比較するとファイルサイズは大きくなりますが、AssetBundle全体を解凍する必要が無い為、読み込み時間はLZMAより早くなります。");
			
		}


		static AssetBundleBuilderEditorWindow Instance;

		enum OptionMode
        {
			Normal,
			Easy,
        }

		enum CompressMode
        {
			LZMA,	// ファイルサイズは小さくなるがファイル全体を解凍する必要がある為、読み込み時間が長くなる
			None,	// ファイルサイズは大きくなるが読み込みは早い
			LZ4,	// LZMAに比較してファイルサイズは大きくなるが、AssetBundle全体を解凍する必要がなく、Asset単位での解凍が行われる
        }


		BuildTarget m_BuildTarget;
		bool m_UncompressedAssetBundle;
		bool m_DisableWriteTypeTree;
		bool m_DeterministicAssetBundle;
		bool m_ForceRebuildAssetBundle;
		bool m_IgnoreTypeTreeChanges;
		bool m_AppendHashToAssetBundleName;
		bool m_ChunkBasedCompression;
		bool m_StrictMode;
		bool m_DryRunBuild;
		bool m_DisableLoadAssetByFileName;
		bool m_DisableLoadAssetByFileNameWithExtension;
		bool m_AssetBundleStripUnityVersion;
		OptionMode m_OptionMode;
		CompressMode m_CompressMode;


		[MenuItem("Window/UTJ/AssetBundleBuilder")]
		public static void Open()
		{
			
			Instance = EditorWindow.GetWindow(typeof(AssetBundleBuilderEditorWindow)) as AssetBundleBuilderEditorWindow;
			Instance.titleContent.text = "AssetBundleBuilder";
		}


		private void OnGUI()
		{
			m_BuildTarget = (BuildTarget)EditorGUILayout.EnumPopup(Styles.BuildTarget, m_BuildTarget);
			var fpath = System.IO.Directory.GetCurrentDirectory();
			fpath = Path.Combine(fpath, "AssetBundles");
			fpath = Path.Combine(fpath, m_BuildTarget.ToString());

			EditorGUILayout.LabelField($"{Styles.OutputPath} {fpath}");

			EditorGUILayout.Separator();

			m_OptionMode = (OptionMode)EditorGUILayout.EnumPopup(Styles.BuildOptions,m_OptionMode);			
			EditorGUI.indentLevel++;
			if (m_OptionMode == OptionMode.Normal)
			{
				m_UncompressedAssetBundle = EditorGUILayout.ToggleLeft(Styles.UncompressedAssetBundle, m_UncompressedAssetBundle);
				m_DisableWriteTypeTree = EditorGUILayout.ToggleLeft(Styles.DisableWriteTypeTree, m_DisableWriteTypeTree);
				m_DeterministicAssetBundle = EditorGUILayout.ToggleLeft(Styles.DeterministicAssetBundle, m_DeterministicAssetBundle);
				m_ForceRebuildAssetBundle = EditorGUILayout.ToggleLeft(Styles.ForceRebuildAssetBundle, m_ForceRebuildAssetBundle);
				m_IgnoreTypeTreeChanges = EditorGUILayout.ToggleLeft(Styles.IgnoreTypeTreeChanges, m_IgnoreTypeTreeChanges);
				m_AppendHashToAssetBundleName = EditorGUILayout.ToggleLeft(Styles.AppendHashToAssetBundleName, m_AppendHashToAssetBundleName);
				m_ChunkBasedCompression = EditorGUILayout.ToggleLeft(Styles.ChunkBasedCompression, m_ChunkBasedCompression);
				m_StrictMode = EditorGUILayout.ToggleLeft(Styles.StrictMode, m_StrictMode);
				m_DryRunBuild = EditorGUILayout.ToggleLeft(Styles.DryRunBuild, m_DryRunBuild);
				m_DisableLoadAssetByFileName = EditorGUILayout.ToggleLeft(Styles.DisableLoadAssetByFileName, m_DisableLoadAssetByFileName);
				m_DisableLoadAssetByFileNameWithExtension = EditorGUILayout.ToggleLeft(Styles.DisableLoadAssetByFileNameWithExtension, m_DisableLoadAssetByFileNameWithExtension);
				m_AssetBundleStripUnityVersion = EditorGUILayout.ToggleLeft(Styles.AssetBundleStripUnityVersion, m_AssetBundleStripUnityVersion);
			}
			else
            {
				m_CompressMode = (CompressMode)EditorGUILayout.EnumPopup(Styles.CompressMode,m_CompressMode);
                switch (m_CompressMode)
                {
					case CompressMode.None:
						m_UncompressedAssetBundle = true;
						m_ChunkBasedCompression = false;
						break;

					case CompressMode.LZMA:
						m_UncompressedAssetBundle = false;
						m_ChunkBasedCompression = false;
						break;

					case CompressMode.LZ4:
						m_UncompressedAssetBundle = false;
						m_ChunkBasedCompression = true;
						break;
				}			
				
				m_ForceRebuildAssetBundle = EditorGUILayout.ToggleLeft(new GUIContent("強制ビルド","全てのAssetBundleを強制的にビルドします"), m_ForceRebuildAssetBundle);
						

			}
			EditorGUI.indentLevel--;

			EditorGUILayout.Separator();


			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button(Styles.Build))
			{
				BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.None;
				if (m_UncompressedAssetBundle) assetBundleOptions |= BuildAssetBundleOptions.UncompressedAssetBundle;
				if (m_DisableWriteTypeTree) assetBundleOptions |= BuildAssetBundleOptions.DisableWriteTypeTree;
				if (m_DeterministicAssetBundle) assetBundleOptions |= BuildAssetBundleOptions.DeterministicAssetBundle;
				if (m_ForceRebuildAssetBundle) assetBundleOptions |= BuildAssetBundleOptions.ForceRebuildAssetBundle;
				if (m_IgnoreTypeTreeChanges) assetBundleOptions |= BuildAssetBundleOptions.IgnoreTypeTreeChanges;
				if (m_AppendHashToAssetBundleName) assetBundleOptions |= BuildAssetBundleOptions.AppendHashToAssetBundleName;
				if (m_ChunkBasedCompression) assetBundleOptions |= BuildAssetBundleOptions.ChunkBasedCompression;
				if (m_StrictMode) assetBundleOptions |= BuildAssetBundleOptions.StrictMode;
				if (m_DryRunBuild) assetBundleOptions |= BuildAssetBundleOptions.DryRunBuild;
				if (m_DisableLoadAssetByFileName) assetBundleOptions |= BuildAssetBundleOptions.DisableLoadAssetByFileName;
				if (m_DisableLoadAssetByFileNameWithExtension) assetBundleOptions |= BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension;
				if (m_AssetBundleStripUnityVersion) assetBundleOptions |= BuildAssetBundleOptions.AssetBundleStripUnityVersion;
				if (!System.IO.Directory.Exists(fpath))
				{
					System.IO.Directory.CreateDirectory(fpath);
				}
				if(m_OptionMode == OptionMode.Easy)
                {
					assetBundleOptions |= BuildAssetBundleOptions.StrictMode;
				}
				var assetbundleManifest = BuildPipeline.BuildAssetBundles(fpath, assetBundleOptions, m_BuildTarget);
				EditorUtility.DisplayDialog("Build AssetBundle", assetbundleManifest != null? "Sucsess" : "Error","OK");
			}

			if (GUILayout.Button(Styles.Copy))
			{
				var dest = Application.streamingAssetsPath;
				if (!System.IO.Directory.Exists(dest))
				{
					System.IO.Directory.CreateDirectory(dest);
				}
				dest = Path.Combine(dest, "AssetBundles");
				if (!System.IO.Directory.Exists(dest))
				{
					System.IO.Directory.CreateDirectory(dest);
				}
				dest = Path.Combine(dest, m_BuildTarget.ToString());
				if (!System.IO.Directory.Exists(dest))
				{
					System.IO.Directory.CreateDirectory(dest);
				}
				CopyDirectory(fpath, dest, true);
				AssetDatabase.Refresh();
			}

			if (GUILayout.Button(Styles.Clear))
			{
				var dest = Application.streamingAssetsPath;
				dest = Path.Combine(dest, "AssetBundles");
				dest = Path.Combine(dest, m_BuildTarget.ToString());
				if (System.IO.Directory.Exists(dest))
				{
					Directory.Delete(dest, true);
					AssetDatabase.Refresh();
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
		{			
			var dir = new DirectoryInfo(sourceDir);			
			if (!dir.Exists)
				throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");			
			DirectoryInfo[] dirs = dir.GetDirectories();

			if (!Directory.Exists(destinationDir))
			{				
				Directory.CreateDirectory(destinationDir);
			}			
			foreach (FileInfo file in dir.GetFiles())
			{
				string targetFilePath = Path.Combine(destinationDir, file.Name);
				file.CopyTo(targetFilePath, true);
			}			
			if (recursive)
			{
				foreach (DirectoryInfo subDir in dirs)
				{
					string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
					CopyDirectory(subDir.FullName, newDestinationDir, true);
				}
			}
		}

	}
}