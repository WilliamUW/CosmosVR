// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

contract SpaceAssets {
    struct SpaceAsset {
        string name;
        string description;
        string textureImageUrl;
        uint256 size;
        uint256 rotation;
        uint256 price;
    }

    mapping(uint256 => SpaceAsset) public assets; // Mapping from asset ID to SpaceAsset
    mapping(address => uint256[]) public ownedAssets; // Mapping from owner address to list of owned asset indices
    uint256 public assetCounter = 0; // Counter for asset IDs

    // Event emitted when a new asset is created
    event AssetCreated(uint256 assetId, string name, address owner);

    // Event emitted when an asset is purchased
    event AssetPurchased(uint256 assetId, address buyer);

    // Constructor to initialize the contract with predefined SpaceAssets
    constructor() {
    }

    // Internal function to create a new space asset
    function createAsset(
        string memory _name,
        string memory _description,
        string memory _textureImageUrl,
        uint256 _size,
        uint256 _rotation,
        uint256 _price
    ) public payable {
        assets[assetCounter] = SpaceAsset(
            _name,
            _description,
            _textureImageUrl,
            _size,
            _rotation,
            _price
        );
        emit AssetCreated(assetCounter, _name, msg.sender);
        assetCounter++;
    }

    // Function to purchase an asset
    function buyAsset(uint256 _assetId, address _owner) public payable {
        require(_assetId < assetCounter, "Asset does not exist");
        // require(msg.value >= assets[_assetId].price, "Insufficient funds");

        ownedAssets[_owner].push(_assetId);
        // payable(address(this)).transfer(msg.value); // Funds stay with the contract

        emit AssetPurchased(_assetId, _owner);
    }

    // Function to retrieve the list of owned asset indices for a specific address
    function getOwnedAssets(address _owner)
        public
        view
        returns (uint256[] memory)
    {
        return ownedAssets[_owner];
    }

    // Function to retrieve details of a specific space asset by index
    function getSpaceAsset(uint256 _assetId)
        public
        view
        returns (SpaceAsset memory)
    {
        require(_assetId < assetCounter, "Asset does not exist");
        return assets[_assetId];
    }

    // Function to retrieve details of all space assets
    function getSpaceAssetAll() public view returns (SpaceAsset[] memory) {
        SpaceAsset[] memory allAssets = new SpaceAsset[](assetCounter);
        for (uint256 i = 0; i < assetCounter; i++) {
            allAssets[i] = assets[i];
        }
        return allAssets;
    }

    function getOwnedAssetsAsString(address _owner)
        public
        view
        returns (string memory)
    {
        uint256[] memory ownerAssets = ownedAssets[_owner];
        if (ownerAssets.length == 0) {
            return "[]";
        }

        bytes memory result = abi.encodePacked("[");
        for (uint256 i = 0; i < ownerAssets.length; i++) {
            if (i > 0) {
                result = abi.encodePacked(result, ",");
            }
            result = abi.encodePacked(result, uintToString(ownerAssets[i]));
        }
        result = abi.encodePacked(result, "]");

        return string(result);
    }

    function getSpaceAssetAllAsString() public view returns (string memory) {
        SpaceAsset[] memory allAssets = getSpaceAssetAll();

        bytes memory result = abi.encodePacked("[");
        for (uint256 i = 0; i < allAssets.length; i++) {
            if (i > 0) {
                result = abi.encodePacked(result, ",");
            }
            result = abi.encodePacked(
                result,
                "{",
                '"name":"',
                allAssets[i].name,
                '",',
                '"description":"',
                allAssets[i].description,
                '",',
                '"textureImageUrl":"',
                allAssets[i].textureImageUrl,
                '",',
                '"size":',
                uintToString(allAssets[i].size),
                ",",
                '"rotation":',
                uintToString(allAssets[i].rotation),
                ",",
                '"price":',
                uintToString(allAssets[i].price),
                "}"
            );
        }
        result = abi.encodePacked(result, "]");

        return string(result);
    }

    function uintToString(uint256 value) internal pure returns (bytes memory) {
        if (value == 0) {
            return "0";
        }
        uint256 temp = value;
        uint256 digits;
        while (temp != 0) {
            digits++;
            temp /= 10;
        }
        bytes memory buffer = new bytes(digits);
        while (value != 0) {
            digits -= 1;
            buffer[digits] = bytes1(uint8(48 + uint256(value % 10)));
            value /= 10;
        }
        return buffer;
    }
}