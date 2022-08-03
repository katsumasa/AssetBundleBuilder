# AssetBundleBuilder
Simple AssetBundle Build

## 概要

シンプルなAssetBundleをビルドする為のGUIです。
何か検証する度に毎回記述するのも面倒なのでという場合にお使い下さい。

<img width="400" alt="image" src="https://user-images.githubusercontent.com/29646672/182540536-cb2ab8ae-b4b0-4227-a81f-74971a1dfb96.png">

## 使い方

### Build Target

AssetBundleのビルドするプラットフォームを選択します。
Editor上でAssetBundleのロード等を検証する時は、UnityEditorが動作しているプラットフォーム(Windows/Mac/Linux)を選択します。

### Output Path

AssetBundleのビルド先を表示しています。プロジェクトフォルダ以下に`AssetBundles/BuildTarget`の名前のフォルダを作成しそこを出力先としています。

### Build Options

AssetBundleビルド時のオプションです。

### Build

設定された内容でAssetBundleをビルドします。

### Copy

ビルドされたAssetBundleをSteramingAssetフォルダーへコピーします。

### Clear

StreamingAssetにコピーされたAssetBundleを削除します。

以上です。
