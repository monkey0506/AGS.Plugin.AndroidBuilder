# AGS.Plugin.AndroidBuilder

Android builder plugin for [Adventure Game Studio](http://www.adventuregamestudio.co.uk/).

This AGS editor plugin will allow you to simply and easily create standalone APK files of your AGS game, which can be published on the Google Play Store.

## CAUTION: Not production ready!

This plugin is still in the early development stages and may have some technical issues. You should **not** use this for a production APK yet.

## Prerequisites

To use this plugin, you will need the following:

* AGS 3.4.1 or higher (*NOTE:* As of this writing, the latest release is 3.4.0. This plugin depends on features which are not yet released. You will need to use a prebuilt editor packaged with this plugin or [rebuild the editor](https://github.com/adventuregamestudio/ags) yourself.)

* [Java Development Kit (JDK)](http://www.oracle.com/technetwork/java/javase/downloads/index.html)

* [Android SDK](https://developer.android.com/studio/index.html) (*NOTE:* Android Studio itself is not required, but it is recommended.)

* A [Java KeyStore](https://en.wikipedia.org/wiki/Keystore) file and alias. This may be generated using the [keytool](https://docs.oracle.com/javase/8/docs/technotes/tools/unix/keytool.html) tool, which is included with the JDK, or using the Android Studio [signing wizard](https://developer.android.com/studio/publish/app-signing.html).

* A zip archive containing your [launcher icons](https://developer.android.com/guide/practices/ui_guidelines/icon_design_launcher.html). This zip may be generated using the [Android Asset Studio](https://romannurik.github.io/AndroidAssetStudio/), or by other means. You must provide (at minimum) the following files:

  * `res/mipmap-hdpi/ic_launcher.png`
  * `res/mipmap-mdpi/ic_launcher.png`
  * `res/mipmap-xhdpi/ic_launcher.png`
  * `res/mipmap-xxhdpi/ic_launcher.png`
  * `res/mipmap-xxxhdpi/ic_launcher.png`

* A [Google Play Developer License](https://developer.android.com/distribute/googleplay/start.html) (if you intend to publish your APK via Google Play). (*NOTE:* There is a *one-time* $25 USD registration fee to become a Google Play Developer. This is per developer, not per app.)

## Setting up the plugin for first use

When you first add this plugin, your game won't be ready out-of-the-box to build for Android. Some assembly is required!

![Android settings](https://bitbucket.org/monkey0506/ags.plugin.androidbuilder/downloads/AndroidSettings.png "Android settings")

Open up the `Android` pane, and you'll be able to add the following information, needed to build your APK.

* **App Name:** The name of your game as it will appear on Google Play. This is also used as the name of your APK file.

* **Package Name:** A valid [Java package name](https://docs.oracle.com/javase/tutorial/java/package/namingpkgs.html) which will be used by the Java files of your Android app.

* **Version Code:** The version number of your Android app.

* **Version Name:** The "*friendly*" version name of your Android app. This can include pretty much any text you want, but it isn't meant as a full description.

* **APK Expansion (OBB) Version:** The version number of your [APK Expansion file](https://developer.android.com/google/play/expansion-files.html). This should be changed when your AGS game has changed. The expansion file will be created for you as part of the build process.

* **Launcher Icon Zip:** The zip archive containing your launcher icons.

* **Key Store Path:** The path to your Java KeyStore file. You must use the **same** key store and alias every time you sign and release your APK.

* **Key Store Password:** The password to your Java KeyStore.

* **Key Store Alias:** The alias in your Java KeyStore used for signing this Android app. You must use the **same** key store and alias every time you sign and release your APK.

* **Key Store Alias Password:** The password for the specified alias in your Java KeyStore.

* **RSA Public Key:** Provided in the [Google Play Developer Console](https://play.google.com/apps/publish/) after you have uploaded your first APK for this app. This value may be left blank, in which case your APK Expansion file will be embedded into your APK. This is not recommended for public releases, because the APK Expansion file must then be copied to external storage before it can be used. The recommended method is to leave this blank only for the initial APK upload.

* **Private Salt:** A comma-separated list of signed byte values (`[-128, 127]`) used as an encryption salt by Google Play services. Once you upload your first APK to Google Play, this value should not be changed.

Finally, make sure to check Android in the `Build target platforms` section of General Settings. After this initial setup, you shouldn't need to change any of these values except for your version info.

## Building

Building your APK is a two-step process, which is currently implemented using some Java command-line tools. These tools don't require any additional interaction from you, but (*technical note*) the plugin currently doesn't check these tools for errors. A future version of the plugin will hopefully make this process more user friendly, but for now you will see two Command Prompt windows pop-up during building:

### jobb tool (APK Expansion File)

![jobb tool (APK Expansion File)](https://bitbucket.org/monkey0506/ags.plugin.androidbuilder/downloads/AndroidJobb.png "jobb tool (APK Expansion File)")

This window shows the output from the [jobb tool](https://developer.android.com/studio/command-line/jobb.html), which is used to create your APK Expansion file. The window will pause so that you can examine the output from the tool. It should display "Success!" if there were no errors creating your APK Expansion file.

(*NOTE:* The `jobb` tool used by the plugin contains a [bugfix](https://code.google.com/p/android/issues/detail?id=220717) for an issue in the tool distributed with the Android SDK. See the [jobbifier project](https://github.com/monkey0506/jobbifier) for source modifications.)

### Gradle (Android Studio project)

![Gradle (Android Studio project)](https://bitbucket.org/monkey0506/ags.plugin.androidbuilder/downloads/AndroidGradle.png "Gradle (Android Studio project)")

Currently the plugin works by creating an [Android Studio](https://developer.android.com/studio/index.html) project for your app, then invoking [Gradle](https://gradle.org/) to build that project. Once the plugin has created the Android Studio project (`Compiled/Android/Studio`), you can use Android Studio to further manage changes to your app. However, bear in mind that changes made to the plugin settings may overwrite changes made to the Android Studio project files. A future version of the plugin will take additional measures to prevent overwriting user changes. The window will pause so that you can examine output from the tool. It should display "BUILD SUCCESSFUL" if there are no errors building your APK.

### Output

Once everything has been built successfully, your APK will be located at `Compiled/Android/Release`. If you have provided an RSA public key, then your APK Expansion file will also be included in this folder (otherwise, it will be embedded into the APK). This file (or files) is ready to be uploaded to Google Play.