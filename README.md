# net-simple-free-pdf-printer

[![Nuget](https://buildstats.info/nuget/SimpleFreePdfPrinter)](https://www.nuget.org/packages/SimpleFreePdfPrinter)

[![.NET Core](https://github.com/PockyBum522/net-simple-free-pdf-printer/actions/workflows/dotnet.yml/badge.svg)](https://github.com/PockyBum522/net-simple-free-pdf-printer/actions/workflows/dotnet.yml)

[![Build history](https://buildstats.info/appveyor/chart/PockyBum522/net-simple-free-pdf-printer)](https://ci.appveyor.com/project/PockyBum522/net-simple-free-pdf-printer/history)

[![GitHub stars](https://img.shields.io/github/stars/PockyBum522/net-simple-free-pdf-printer.svg?style=social&label=Star&maxAge=2592000)](https://GitHub.com/PockyBum522/net-simple-free-pdf-printer/stargazers/)

Please star if you find this useful!

A simple way to print PDFs on Windows without needing to have Adobe Reader installed. .NET 6 library

Example if you just want to print:

    Set your target framework to net6.0-windows and a 64-bit project, then:

    var pdfPrinter = new Jds2.SimpleFreePdfPrinter();
                
    pdfPrinter.PrintPdfToDefaultPrinter(@"..\net5.0-windows\Resources\SamplePdfFile.pdf");
                
    // Document should print, two pages, with pages filled, from system default printer in Windows when this test is run.

    // There is also pdfPrinter.PrintPdfTo, if you want to specify a printer by name.

~~~~

// To convert:

var pdfConverter = new PdfToBitmapListConverter();

// Output a list of paths in Temp directory, one PNG per page on disk:
List<string> pngPathsList = pdfConverter.GetPdfPagesAsPngs(string pdfFileToWork);  

// Output a list of paths in Temp directory, one PNG per page on disk:
List<Bitmap> bitmapList = GetPdfPagesAsBitmapList(string pdfFileToWork)
    
~~~~

Most of the libraries to do this are several hundred dollars for a license. This is free and released under MIT license. If this saved you some time, feel free to throw a few bucks my way. I'll think of you when I'm having a beer.

Paypal: me@dsikes.com

Licensed under MIT License:
    
    Copyright 2021 David Sikes/PockyBum522

    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Dependencies: None, but code borrowed heavily under license from below, and respective files contain their original notices:
        
    Pdf2Png:

    Original License:
        SPDX identifier
        MIT

        License text
        MIT License _____

        Permission is hereby granted, free of charge, to any person obtaining a copy of _____ (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

        The above copyright notice and this permission notice (including the next paragraph) shall be included in all copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL _____ BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
