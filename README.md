# ByteTiles

![ByteTiles][product-screenshot]

Parse **.mbtiles** file in a format readable by [Amazon S3](https://aws.amazon.com/s3/). 

An **.mbtiles** file contains a SQLite database and needs to be loaded in memory before requesting data. 
Instead a **.bytetiles** file constains a list of tiles and a dictionary (tile key and byte range) that indicates the position in the file for every tile.


## Bennefits

* Reduce cost of serving map tiles.
* Reduce storage.
* Easy to update. Instead of extracting the files contained in **.mbtiles** and the upload them all to S3, a **.bytetile** file contains all in a single file.


## Built With

* [MBTiles](https://wiki.openstreetmap.org/wiki/MBTiles)
* C#
* JavasScript#


<!-- DEMO EXAMPLES -->
## Examples

Examples are contained in the **SimpleByteTilesServer** and **ByteTilesReaderWriter_Test** projects.

<!-- INSTALATION -->
## Installation

1. Clone the repo
   ```sh
   git clone https://github.com/techjb/Vector-Tiles-Google-Maps.git
   ```
2. Install NPM packages
   ```sh
   npm install
   ```

<!-- Content -->
## Content
The package contains the following proyects:

* *ByteTilesReaderWriter*: Library to parse a **.mbtiles** file to **.bytetiles** file and read tiles data.
* *ByteTilesReaderWriter_Test*: Test for the **ByteTilesReaderWriter** library.
* *SimpleByteTilesServer*: Simple ByteTiles server with examples.
* *ByteTilesLogo*: Logo files
* *ByteTilesSpec*: ByteTiles specification

<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/techjb/ByteTiles/issues) for a list of proposed features (and known issues).


<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<!-- LICENSE -->
## License

See [license](https://github.com/techjb/ByteTiles/blob/master/LICENSE.txt) for more information.


<!-- CONTACT -->
## Contact

Jesús Barrio - [@techjb](https://twitter.com/techjb)

Project Link: [https://github.com/techjb/ByteTiles](https://github.com/techjb/ByteTiles)

<!-- MARKDOWN LINKS & IMAGES -->

[product-screenshot]: ByteTilesLogo/logo_small.png