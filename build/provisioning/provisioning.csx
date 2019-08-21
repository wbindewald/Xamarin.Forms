var channel = Env("CHANNEL") ?? "Stable";

if (IsMac)
{
  Item (XreItem.Xcode_10_1_0).XcodeSelect ();
}
Console.WriteLine(channel);
XamarinChannel(channel);

AndroidSdk ()
	.ApiLevel (AndroidApiLevel.JellyBean)
	.ApiLevel (AndroidApiLevel.JellyBean_4_2)
	.ApiLevel (AndroidApiLevel.JellyBean_4_3)
	.ApiLevel (AndroidApiLevel.KitKat)
	.ApiLevel (AndroidApiLevel.Lollipop)
	.ApiLevel (AndroidApiLevel.Lollipop_5_1)
	.ApiLevel (AndroidApiLevel.Lollipop_5_2)
	.ApiLevel (AndroidApiLevel.Marshmallow)
	.ApiLevel (AndroidApiLevel.Nougat)
	.ApiLevel (AndroidApiLevel.Nougat_7_1)
	.ApiLevel (AndroidApiLevel.Oreo)
	.ApiLevel (AndroidApiLevel.Oreo_8_1)
	.ApiLevel (AndroidApiLevel.P)
	.SdkManagerPackage ("build-tools;27.0.0")
	.SdkManagerPackage ("extras;google;m2repository");