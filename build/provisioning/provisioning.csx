var channel = Env("CHANNEL") ?? "Internal Preview";

if (IsMac)
{
  Item (XreItem.Xcode_10_3_0).XcodeSelect ();
}
Console.WriteLine(channel);
XamarinChannel(channel);

AndroidSdk ()
	.ApiLevel (AndroidApiLevel.Oreo)
	.ApiLevel (AndroidApiLevel.Oreo_8_1)
	.ApiLevel (AndroidApiLevel.P)
	.ApiLevel (AndroidApiLevel.Q)
	.SdkManagerPackage ("build-tools;27.0.0")
	.SdkManagerPackage ("extras;google;m2repository");