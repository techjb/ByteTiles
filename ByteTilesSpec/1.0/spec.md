# ByteTiles 1.0

The key words "MUST", "MUST NOT", "REQUIRED", "SHALL", "SHALL NOT",
"SHOULD", "SHOULD NOT", "RECOMMENDED", "MAY", and "OPTIONAL" in
this document are to be interpreted as described in [RFC 2119](https://www.ietf.org/rfc/rfc2119.txt).

## Abstract

ByteTiles is a specification for storing tiled map data for immediate usage and for transfer.
ByteTiles files, MUST implement the specification below to ensure compatibility with devices.

## Charset

All text a ByteTiles file MUST be encoded as UTF-8.

## Differences with MBTiles

ByteTiles is thinked to be created from an mbtiles file (See the [MBTiles 1.3 Specification](https://github.com/mapbox/mbtiles-spec/blob/master/1.3/spec.md)). Differences are:

* MBTiles follows the [TMS](https://wiki.osgeo.org/wiki/Tile_Map_Service_Specification) scheme and ByteTiles instead
follows the XYZ scheme. See how [to convert](https://gist.github.com/tmcw/4954720).

* The `medatada` table in mbtiles is a key-value json in ByteTiles.

## Definitions

### Tile key

A tile key is the XYZ identifier separated by `/` character. 
For example the tile key `8015/6171/14` corresponds to x: 8015, y: 6171, z: 14.

### Byte range

A byte range is defined by a *position* from the beggining of the file and *length* in bytes separated by `-` character. 
The byte range `822834-234` indicates the position `822834` and a length of `234` bytes.

### Header

Json containing metadata of the ByteTiles file. We call it *header* to avoid misunderstandings with the *metadata* mbtiles table.

* The header MUST contains a key named `version` with the ByteTiles version specification.

* The header MUST constain a key named `metadata` with the byte range of the metadata json.

* The header MUST constain a key named `tiles_dictionary` with the byte range of the table tiles dictionary.

* The header MAY contain a key named `grids_dictionary` with the byte range of the table grids dictionary.

* The header MAY contain a key named `grid_data_dictionary` with the byte range of the table grid_data dictionary.

Header example:

```
{
   "version":"1.0",
   "metadata":"8170242-538",
   "tiles_dictionary":"8137245-32997"
}
```

### Dictionary

A key-value pair json that matches a tile key with a byte range. Dictionary example:

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

## Bytes reserved

The last 50 bytes in a ByteTiles file MUST be reserved for the byte range of header. It will be used for the first time 
when accesing the file in order to read the rest of the data. Empty space are filled with ` `. Example:

```
8170780-80                                        
```

## File content

The content of a ByteTiles file MUST be the same as the content of a MBTiles with the following exception:

* The table `metadata` in the MBTiles is parsed to a key-value list in json format.

The columns `tile_column`, `tile_row` and `zoom_level` in the MBTiles file, are used to create a tile key following the XYZ schema.

Decompression for the column `tile_data` in the MBTiles is OPTIONAL (for example for `pbf` compressed content).


## Reading metadata

The flow for reading the `metadata` table in a ByteTiles file is:

1. Read the las 50 bytes to get the byte range of *header*.
2. Read *header* and get the byte range of the key `medatada`.
3. Read the *metadata*.

## Reading tiles

The flow for reading a XYZ tile in a ByteTiles file is:

1. Read the las 50 bytes to get the byte range of *header*.
2. Read *header* and get the byte range of the key `tiles_dictionary`.
3. Read the *tiles dictionary* and get the byte range for the *XYZ* key. 
4. Read the *tile*.

