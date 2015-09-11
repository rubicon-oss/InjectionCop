# InjectionCop


InjectionCop facilitates secure coding through static code analysis and contract annotations in the Common Language Infrastructure.

In order to avoid injection scenarios, such as SQL injection or HTML XSS, InjectionCop creates warnings when unverified strings are passed to methods which are considered dangerous, such as HtmlTextWriter.Write or IDbCommand.Text. By default, only constants are considered verified.

````c#
public void Foo (string param)
{
  HtmlWriter writer = ...;
  
  writer.Write (param);     // warning: rendering an unsafe paramter)

  writer.Write ("literal"); // no warning: rendering a literal value)

  const string constant = "constant";
  writer.Write (constant);  // no warning: rendering a constant value

  writer.Write ("literal " + constant); // no warning: rendering an expression made up of constant values only (we're planning to detect a few scenarios, but not all)
} 
````

The developer can explicitly mark parameters to avoid warnings, thus passing the warning to the calling method. E.g. a method SafeFoo (string input) that passes its parameter to HtmlTextWriter.Write would have to mark this parameter as unsafe. Another method that calls Foo() with a non-constant value for 'input' would therefore raise a warning.

````c#
public void SafeFoo ([HtmlFragment] string param)
{
  HtmlWriter writer = ...;

  writer.Write (param);   // no warning: rendering a parameter declared as HTML fragment
}

public void Bar ([HtmlFragment] string safeParam, string unsafeParam)
{
  SafeFoo (safeParam);    // no warning: passing a paramter declared as HTML fragment
  SafeFoo (unsafeParam);  // warning: passing an unsafe parameter
  SafeFoo ("literal");    // no warning: passing a literal value
  Foo (<anything>);       // no warning: calling a method that already generated a warning 
}
````

For special cases, a developer can mark any call to an unsafe method as trusted. In this case, warnings will be omitted. (In order to verify usage of these markings, you just need to find usages of these marker attributes.)
The following example retreives a value from the config file, which the developer declares to be safe:

````c#
[TrustedHtmlGeneration]
[return: HtmlFragment]
public string RetreiveSafeValue ()
{
  return AppConfig.Settings["HtmlString"]; // no warning: method is marked as trusted 
}

public void Bar2 ()
{
  string value = RetreiveSafeValue ();
  SafeFoo (value); // no warning: value was passed from a method that is declared to return an HTML fragment
}
````



Previously located at: https://injectioncop.codeplex.com/
