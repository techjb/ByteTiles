# ByteTiles 1.0

The key words "MUST", "MUST NOT", "REQUIRED", "SHALL", "SHALL NOT",
"SHOULD", "SHOULD NOT", "RECOMMENDED", "MAY", and "OPTIONAL" in
this document are to be interpreted as described in [RFC 2119](https://www.ietf.org/rfc/rfc2119.txt).

## Abstract

ByteTiles is a specification for storing tiled map data for immediate usage and for transfer.
ByteTiles files, MUST implement the specification below to ensure compatibility with devices.

## Charset

All text in `text` columns of tables in an mbtiles tileset MUST be encoded as UTF-8.

## MBTiles 

ByteTiles is thinked to be created from an mbtiles file. See the [MBTiles 1.3 Specification](https://github.com/mapbox/mbtiles-spec/blob/master/1.3/spec.md).

MBTiles follows the [Tile Map Service Specification](https://wiki.osgeo.org/wiki/Tile_Map_Service_Specification) (TMS) scheme. ByteTiles instead
follows the XYZ scheme. See how [to convert](https://gist.github.com/tmcw/4954720).

## Byte range

A byte range is defined by a start position and length in bytes separated by `-` character. 
For example the byte range `822834-234` indicates the position `822834` from the start of the file and a
length of `234` bytes.

## Tile key

A tile key is the XYZ unique identifier for a tile separated by `/` character. 
For example the tile key `8015/6171/14` corresponds to X: 8015, Y: 6171, Z: 14.

## Tiles dictionary

A key-value pair json that matched a tile key to a byte range. Example:

```
{
    "0/0/0":"0-14311",
    "10/5/4":"14311-16073",
    "10/4/4":"30384-10649",
    "10/3/4":"41033-12345",
    "5/22/5":"53378-2785",
    "5/21/5":"56163-2182",
    "5/20/5":"58345-2471",
    "7/1/5":"60816-1448",
    "10/12/5":"62264-2573",
    "7/0/5":"64837-1452"
}

```
