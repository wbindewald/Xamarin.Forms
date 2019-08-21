var channel = Env("CHANNEL") ?? "Internal Preview";

if (IsMac)
{
  Item (XreItem.Xcode_10_3_0).XcodeSelect ();
}
Console.WriteLine(channel);
XamarinChannel(channel);
