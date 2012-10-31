As WinRT isn't fully supported in NuGet yet, you'll need to manually add a reference
to the MetroLog.WinRT.winmd file in your \packages\MetroLog.*\netcore45 directory. Just 
add the reference to the WinMD file, not anything else in the directory.

TypeScript definitions are provided as content in the package as well.

To hook up the debug logger to your WinJS console, in your default.js file, add the 
following at the top, right under 'var nav = WinJS.Navigation;'

MetroLog.WinRT.Logger.addEventListener("onlogmessage", function(message) {
        Debug.writeln(message);
    });

Then, in each module/js file you can add the following, putting in a useful name
for the logger, such as the relative file name.

var logger = MetroLog.WinRT.Logger.getLogger("pages/myfile.js");

