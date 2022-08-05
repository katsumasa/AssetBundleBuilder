# AssetBundleBuilder

![GitHub package.json version](https://img.shields.io/github/package-json/v/katsumasa/AssetBundleBuilder)

Simple AssetBundle Build GUI

## 概要

シンプルなAssetBundleをビルドする為のGUIです。
何か検証する度に毎回記述するのも面倒なのでという場合にお使い下さい。

### Normalモード

<img width="400" alt="image" src="https://user-images.githubusercontent.com/29646672/182973110-d79f68ea-b8fa-45fe-a8bd-a7d0ceb379c2.png">

### Easyモード

<img width="400" alt="image" src="https://user-images.githubusercontent.com/29646672/182973200-4cff1393-9f6f-48af-800e-17a7d6428c37.png">


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

### Clear AB

ビルド先のAssetBundleを削除します。

### Clear StreamingAssets

StreamingAssetにコピーされたAssetBundleを削除します。

以上です。
