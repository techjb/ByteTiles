# ByteTiles

![ByteTiles][product-screenshot]

File format for storing tilesets. Mainly designed to fetch tiles by a range of bytes.

A [MBTiles](https://docs.mapbox.com/help/glossary/mbtiles/) file contains a SQLite database and needs to be loaded in memory before requesting data. 
Instead a **ByteTiles** file constains a list of tiles and a dictionary (tile key - byte range) that indicates the position in the file for any tile. 
The dictionary is thinked to be loaded in memory and then used to locate the tiles in the file.


## Bennefits

* Reduce cost of serving map tiles: a **.bytetiles** file can be uploaded in [Amazon S3](https://aws.amazon.com/s3/) and then fetch tiles by it range of bytes.
* Reduce storage size (**.bytetiles** files are 5-10% smaller than **.mbtiles** files).
* Faster to update: instead of extracting the files contained in **.mbtiles** and upload them all to S3, a **.bytetile** file contains all in a single file.


## Built With

* [MBTiles](https://wiki.openstreetmap.org/wiki/MBTiles)
* C#
* JavasScript#


## Specification

ByteTiles specification is provided in the [ByteTilesSpec](https://github.com/techjb/ByteTiles/tree/master/ByteTilesSpec) folder.

## Examples

Examples are contained in the **SimpleByteTilesServer** and **ByteTilesReaderWriter_Test** projects.


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

* *ByteTilesReaderWriter*: Library to parse a **.mbtiles** file to **.bytetiles** file and read tiles data.
* *ByteTilesReaderWriter_Test*: Test for the **ByteTilesReaderWriter** library.
* *SimpleByteTilesServer*: Simple ByteTiles server with examples.
* *ByteTilesLogo*: ByteTiles logo files.
* *ByteTilesSpec*: ByteTiles specifications.

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

<!-- MARKDOWN LINKS & IMAGES -->

[product-screenshot]: ByteTilesLogo/logo_small.png