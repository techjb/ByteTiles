<p align="center">
  <img src="ByteTilesLogo/logo.svg" height="256">  
</p>

# ByteTiles

File format for storing [TileSets](https://docs.mapbox.com/help/glossary/tileset/) and designed to be accessed by byte range.

A [MBTiles](https://docs.mapbox.com/help/glossary/mbtiles/) file contains a SQLite database and needs to be loaded in memory before requesting data. 
Instead a **ByteTiles** file constains a list of tiles and a dictionary (tile key - byte range) that indicates the position in the file for any tile. 
The dictionary is thinked to be loaded in a fast memory access (cache or database) for a fast tile reading.


## Bennefits

* Cost: A **ByteTiles** file can be uploaded in [Amazon S3](https://aws.amazon.com/s3/) and then fetch tiles by it range of bytes.
* Storage size: A **ByteTiles** files is 5-10% smaller than **MBTiles** file.
* Updates: Instead of extracting the files contained in **MBTiles** and upload them all to S3, **ByteTiles** contains all in a single file.


## Built With

* C#
* JavasScript


## Specification

ByteTiles specification is provided in the *ByteTilesSpec* folder.

* [Version 1.0](https://github.com/techjb/ByteTiles/blob/master/ByteTilesSpec/1.0/spec.md)

## Examples

Some examples are contained in the **SimpleByteTilesServer** and **ByteTilesReaderWriter_Test** projects.


## Installation

1. Clone the repo
   ```sh
   git clone https://github.com/techjb/Vector-Tiles-Google-Maps.git
   ```
2. Install NPM packages
   ```sh
   npm install
   ```

## Content
The package contains the following directories:

* *ByteTilesReaderWriter*: Library to parse a **.mbtiles** file to **.bytetiles** file, read tiles and extract files to folder.
* *ByteTilesReaderWriter_Test*: Test for the **ByteTilesReaderWriter** library.
* *SimpleByteTilesServer*: Simple ByteTiles server with examples.
* *ByteTilesLogo*: ByteTiles logo files.
* *ByteTilesSpec*: ByteTiles specifications.

## Library usage

Some examples for the use of the library *ByteTilesReaderWriter*.

Parse .mbtiles file to .bytetiles file:
```csharp
ByteTilesWriter.ParseMBTiles("input_file.mbtiles", "output_file.bytetiles");
```

Read tiles:
```csharp
var byteTilesReader = new ByteTilesReader("input_file.bytetiles");
byte[] tile = byteTilesReader.GetTile(x, y, z);
```

Read tiles dictionary:
```csharp
var byteTilesReader = new ByteTilesReader("input_file.bytetiles");
var tilesDictionary = byteTilesReader.GetTilesDictionary();
```

Read json metadata:
```csharp
var byteTilesReader = new ByteTilesReader("input_file.bytetiles");
var metadata = byteTilesReader.GetMetadata();
```

Extract .bytetiles files to directory.
```csharp
var byteTilesExtractor = new ByteTilesExtractor("input_file.bytetiles");
byteTilesExtractor.ExtractFiles("output_directory");
```

## Roadmap

See the [open issues](https://github.com/techjb/ByteTiles/issues) for a list of proposed features (and known issues).


## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

See [license](https://github.com/techjb/ByteTiles/blob/master/LICENSE.txt) for more information.


## Contact

Jesús Barrio - [@techjb](https://twitter.com/techjb)

Project Link: [https://github.com/techjb/ByteTiles](https://github.com/techjb/ByteTiles)
