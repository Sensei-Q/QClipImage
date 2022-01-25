// QClipImage v1.0 (c) 2022 Sensei (aka 'Q')
// Clip the image to the desired dimensions.
//
// Usage:
// QClipImage [-h|--help|/?] [-v|--verbose] [-x <x>] [-y <y>] [--width <width>] [--height <height>] [-out|--output <output_filename>] input_image_filename
//
// Compilation:
// %SYSTEMROOT%\Microsoft.NET\Framework\v3.5\csc QClipImage.cs
//
// Examples:
// QClipImage -x 0 -y 0 -w 1000 -h 1000 -out output.png picture.png

using System;
using System.Drawing;
using System.IO;

public class QClipImage {
   public class Options {
      public string input_image_filename;
      public string output_filename;
      public bool verbose;
      public int x;
      public int y;
      public int width;
      public int height;
      public Options() {
         input_image_filename = "";
         output_filename = "";
         x = 0;
         y = 0;
         width = 0;
         height = 0;
      }

      public void parseArgs( string [] args ) {
         for( int i = 0; i < args.Length; i++ ) {
            string arg = args[i];
            if( arg.Equals( "-h" ) || arg.Equals( "--help" ) || arg.Equals( "/?" ) ) {
               Help();
               Environment.Exit( 0 );
            } else if( arg.Equals( "-v" ) || arg.Equals( "--verbose" ) ) {
               verbose = true;
            } else if( arg.Equals( "-x" ) ) {
               i++;
               try {
                  x = Int32.Parse( args[ i ] );
                  if( x < 0 ) throw( new FormatException() );
               } catch( Exception e ) {
                  Console.Error.WriteLine( e.Message );
                  Environment.Exit( 20 );
               }
            } else if( arg.Equals( "-y" ) ) {
               i++;
               try {
                  y = Int32.Parse( args[ i ] );
                  if( y < 0 ) throw( new FormatException() );
               } catch( Exception e ) {
                  Console.Error.WriteLine( e.Message );
                  Environment.Exit( 20 );
               }
            } else if( arg.Equals( "--width" ) ) {
               i++;
               try {
                  width = Int32.Parse( args[ i ] );
                  if( width <= 0 ) throw( new FormatException() );
               } catch( Exception e ) {
                  Console.Error.WriteLine( e.Message );
                  Environment.Exit( 20 );
               }
            } else if( arg.Equals( "--height" ) ) {
               i++;
               try {
                  height = Int32.Parse( args[ i ] );
                  if( height <= 0 ) throw( new FormatException() );
               } catch( Exception e ) {
                  Console.Error.WriteLine( e.Message );
                  Environment.Exit( 20 );
               }
            } else if( arg.Equals( "-out" ) || arg.Equals( "--output" ) ) {
               i++;
               try {
                  output_filename = args[ i ];
               } catch( Exception e ) {
                  Console.Error.WriteLine( e.Message );
                  Environment.Exit( 20 );
               }
            } else if( i == args.Length - 1 ) {
               input_image_filename = arg;
            } else {
               Console.Error.WriteLine( "Unknown argument \"{0}\"!", arg );
               Environment.Exit( 20 );
            }
         }
         if( String.IsNullOrEmpty( input_image_filename ) ) {
            Console.Error.WriteLine( "Required argument is missing!" );
            Environment.Exit( 20 );
         }
         if( String.IsNullOrEmpty( output_filename ) ) {
            output_filename = input_image_filename;
         }
      }
   }

   public static void ClipImage( string input_image_filename, string output_filename, Options options ) {
      if( File.Exists( input_image_filename ) ) {
        try {
           Image image = Image.FromFile( input_image_filename );
           int x = options.x;
           int y = options.y;
           int width = options.width;
           int height = options.height;
           if( x >= image.Width ) throw( new ArgumentException() );
           if( y >= image.Height ) throw( new ArgumentException() );
           if( ( x + width ) > image.Width ) {
              width = image.Width - x;
           }
           if( ( y + height ) > image.Height ) {
              height = image.Height - y;
           }
           Bitmap bitmap = new Bitmap( width, height );
           Graphics graphics = Graphics.FromImage( bitmap );
           graphics.DrawImage( image,
              new RectangleF( 0, 0, width, height ),
              new RectangleF( x, y, width, height ),
              GraphicsUnit.Pixel );
           image.Dispose();
           bitmap.Save( output_filename );
        } catch( Exception ) {
           throw;
        }
      } else {
         throw( new Exception( String.Format( "File \"{0}\" does not exist!", input_image_filename ) ) );
      }
   }

   public static void Help() {
      Console.WriteLine( "QClipImage v1.0 (c) 2022 Sensei (aka 'Q')" );
      Console.WriteLine( "Clip the image to the desired dimensions." );
      Console.WriteLine();
      Console.WriteLine( "Usage:" );
      Console.WriteLine( "QClipImage [-h|--help|/?] [-v|--verbose] [-x <x>] [-y <y>] [--width <width>] [--height <height>] [-out|--output <output_filename>] input_image_filename" );
      Console.WriteLine();
      Console.WriteLine( "Examples:" );
      Console.WriteLine( "QClipImage -x 0 -y 0 -w 1000 -h 1000 -out output.png picture.png" );
   }

   public static void Main( string [] args ) {
      if( args.Length > 0 ) {
         try {
            Options options = new Options();
            options.parseArgs( args );
            ClipImage( options.input_image_filename, options.output_filename, options );
         } catch( Exception e ) {
            Console.Error.WriteLine( e.Message );
            System.Environment.Exit( 20 );
         }
      } else {
         Help();
      }
   }
}
