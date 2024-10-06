require("dotenv").config();

const express = require("express");
const { TronWeb } = require("tronweb");
const app = express();
const PORT = 3000;

// TronWeb configuration for Nile testnet
const fullNode = "https://api.nileex.io";
const solidityNode = "https://api.nileex.io";
const eventServer = "https://api.nileex.io";
const privateKey = process.env.PRIVATE_KEY;
console.log(privateKey);
const tronWeb = new TronWeb(fullNode, solidityNode, eventServer, privateKey);
const contractAddress = "TKswLANpsiY6eAseRuNAgaqe2zgVue11Ai";
const contractAbi = [
  { inputs: [], stateMutability: "nonpayable", type: "constructor" },
  {
    anonymous: false,
    inputs: [
      {
        indexed: false,
        internalType: "uint256",
        name: "assetId",
        type: "uint256",
      },
      { indexed: false, internalType: "string", name: "name", type: "string" },
      {
        indexed: false,
        internalType: "address",
        name: "owner",
        type: "address",
      },
    ],
    name: "AssetCreated",
    type: "event",
  },
  {
    anonymous: false,
    inputs: [
      {
        indexed: false,
        internalType: "uint256",
        name: "assetId",
        type: "uint256",
      },
      {
        indexed: false,
        internalType: "address",
        name: "buyer",
        type: "address",
      },
    ],
    name: "AssetPurchased",
    type: "event",
  },
  {
    inputs: [],
    name: "assetCounter",
    outputs: [{ internalType: "uint256", name: "", type: "uint256" }],
    stateMutability: "view",
    type: "function",
  },
  {
    inputs: [{ internalType: "uint256", name: "", type: "uint256" }],
    name: "assets",
    outputs: [
      { internalType: "string", name: "name", type: "string" },
      { internalType: "string", name: "description", type: "string" },
      { internalType: "string", name: "textureImageUrl", type: "string" },
      { internalType: "uint256", name: "size", type: "uint256" },
      { internalType: "uint256", name: "rotation", type: "uint256" },
      { internalType: "uint256", name: "price", type: "uint256" },
    ],
    stateMutability: "view",
    type: "function",
  },
  {
    inputs: [
      { internalType: "uint256", name: "_assetId", type: "uint256" },
      { internalType: "address", name: "_owner", type: "address" },
    ],
    name: "buyAsset",
    outputs: [],
    stateMutability: "payable",
    type: "function",
  },
  {
    inputs: [
      { internalType: "string", name: "_name", type: "string" },
      { internalType: "string", name: "_description", type: "string" },
      { internalType: "string", name: "_textureImageUrl", type: "string" },
      { internalType: "uint256", name: "_size", type: "uint256" },
      { internalType: "uint256", name: "_rotation", type: "uint256" },
      { internalType: "uint256", name: "_price", type: "uint256" },
    ],
    name: "createAsset",
    outputs: [],
    stateMutability: "payable",
    type: "function",
  },
  {
    inputs: [{ internalType: "address", name: "_owner", type: "address" }],
    name: "getOwnedAssets",
    outputs: [{ internalType: "uint256[]", name: "", type: "uint256[]" }],
    stateMutability: "view",
    type: "function",
  },
  {
    inputs: [{ internalType: "address", name: "_owner", type: "address" }],
    name: "getOwnedAssetsAsString",
    outputs: [{ internalType: "string", name: "", type: "string" }],
    stateMutability: "view",
    type: "function",
  },
  {
    inputs: [{ internalType: "uint256", name: "_assetId", type: "uint256" }],
    name: "getSpaceAsset",
    outputs: [
      {
        components: [
          { internalType: "string", name: "name", type: "string" },
          { internalType: "string", name: "description", type: "string" },
          { internalType: "string", name: "textureImageUrl", type: "string" },
          { internalType: "uint256", name: "size", type: "uint256" },
          { internalType: "uint256", name: "rotation", type: "uint256" },
          { internalType: "uint256", name: "price", type: "uint256" },
        ],
        internalType: "struct SpaceAssets.SpaceAsset",
        name: "",
        type: "tuple",
      },
    ],
    stateMutability: "view",
    type: "function",
  },
  {
    inputs: [],
    name: "getSpaceAssetAll",
    outputs: [
      {
        components: [
          { internalType: "string", name: "name", type: "string" },
          { internalType: "string", name: "description", type: "string" },
          { internalType: "string", name: "textureImageUrl", type: "string" },
          { internalType: "uint256", name: "size", type: "uint256" },
          { internalType: "uint256", name: "rotation", type: "uint256" },
          { internalType: "uint256", name: "price", type: "uint256" },
        ],
        internalType: "struct SpaceAssets.SpaceAsset[]",
        name: "",
        type: "tuple[]",
      },
    ],
    stateMutability: "view",
    type: "function",
  },
  {
    inputs: [],
    name: "getSpaceAssetAllAsString",
    outputs: [{ internalType: "string", name: "", type: "string" }],
    stateMutability: "view",
    type: "function",
  },
  {
    inputs: [
      { internalType: "address", name: "", type: "address" },
      { internalType: "uint256", name: "", type: "uint256" },
    ],
    name: "ownedAssets",
    outputs: [{ internalType: "uint256", name: "", type: "uint256" }],
    stateMutability: "view",
    type: "function",
  },
];
async function getTokenDetails() {
  try {
    const contract = await tronWeb.contract(contractAbi, contractAddress);
    // console.log(contract);
    const getSpaceAssetAll = await contract.getSpaceAssetAllAsString().call();
    console.log("getSpaceAssetAll:", getSpaceAssetAll.toString());

    const getOwnedAssetsAsString = await contract
      .getOwnedAssetsAsString("TGcb8vsyQEaXzZmMnLZCrGK3HJNVgWsmU1")
      .call();
    console.log("getOwnedAssetsAsString:", getOwnedAssetsAsString.toString());
  } catch (error) {
    console.error("Error fetching token details:", error);
  }
}
// getTokenDetails()
//   .then(() => process.exit(0))
//   .catch((error) => {
//     console.error(error);
//     process.exit(1);
//   });

app.get("/", async (req, res) => {
  try {
    res.json({
      result: `Server running!`,
    });
  } catch (error) {
    console.error("Error in /owned-assets route:", error);
    res.status(500).json({
      error: "Failed to fetch owned assets",
      details: error.message,
      stack: error.stack,
    });
  }
});

app.get("/allAssets", async (req, res) => {
  console.log("Received request for /allassets");
  try {
    const contract = await tronWeb.contract(contractAbi, contractAddress);
    // console.log(contract);
    const getSpaceAssetAll = await contract.getSpaceAssetAllAsString().call();
    console.log("getSpaceAssetAll:", getSpaceAssetAll.toString());

    res.json({ result: getSpaceAssetAll });
  } catch (error) {
    console.error("Error in /owned-assets route:", error);
    res.status(500).json({
      error: "Failed to fetch owned assets",
      details: error.message,
      stack: error.stack,
    });
  }
});

app.get("/owned-asset-indices/:address", async (req, res) => {
  console.log("Received request for /owned-assets");
  try {
    const contract = await tronWeb.contract(contractAbi, contractAddress);

    const ownerAddress = req.params.address;
    console.log("Owner address:", ownerAddress);

    const getOwnedAssetsAsString = await contract
      .getOwnedAssetsAsString(ownerAddress)
      .call();
    console.log("getOwnedAssetsAsString:", getOwnedAssetsAsString.toString());

    res.json({ result: getOwnedAssetsAsString });
  } catch (error) {
    console.error("Error in /owned-assets route:", error);
    res.status(500).json({
      error: "Failed to fetch owned assets",
      details: error.message,
      stack: error.stack,
    });
  }
});

app.get("/owned-assets/:address", async (req, res) => {
  console.log("Received request for /owned-assets");
  try {
    const contract = await tronWeb.contract(contractAbi, contractAddress);

    const ownerAddress = req.params.address;
    console.log("Owner address:", ownerAddress);

    const getSpaceAssetAll = await contract.getSpaceAssetAllAsString().call();
    console.log("getSpaceAssetAll:", getSpaceAssetAll.toString());

    const getOwnedAssetsAsString = await contract
      .getOwnedAssetsAsString(ownerAddress)
      .call();
    console.log("getOwnedAssetsAsString:", getOwnedAssetsAsString.toString());

    // Parse the input strings
    const spaceAssets = JSON.parse(getSpaceAssetAll.toString().replaceAll(`\"b\"`,""));
    const ownedAssets = JSON.parse(getOwnedAssetsAsString.toString());

    // Function to get names of owned assets
    function getOwnedAssetNames(spaceAssets, ownedAssets) {
      return ownedAssets.map((index) => spaceAssets[index].name);
    }

    // Get the names of owned assets
    const ownedAssetNames = getOwnedAssetNames(spaceAssets, ownedAssets);

    console.log(ownedAssetNames); // Output: ["Sun"]

    res.json({ result: ownedAssetNames });
  } catch (error) {
    console.error("Error in /owned-assets route:", error);
    res.status(500).json({
      error: "Failed to fetch owned assets",
      details: error.message,
      stack: error.stack,
    });
  }
});

app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});
