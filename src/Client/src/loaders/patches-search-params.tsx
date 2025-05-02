import { AssetFileType } from "@/api/exvs";
import { commonPaginatedSearchParams } from "@/loaders/common-paginated-search-params";
import {
  createLoader,
  createParser,
  parseAsArrayOf,
  parseAsInteger,
  parseAsString,
  UrlKeys,
} from "nuqs";
import { zAssetFileType } from "@/api/exvs/zod.gen";

const parseAsAssetFileType = createParser({
  parse(queryValue) {
    if (
      Object.values(zAssetFileType.Enum).includes(queryValue as AssetFileType)
    ) {
      return queryValue as AssetFileType;
    }
    return zAssetFileType.Enum.Unknown;
  },
  serialize(value) {
    return value;
  },
});

const paginatedPatchesSearchParams = {
  ...commonPaginatedSearchParams,
  assetFileHashes: parseAsArrayOf(parseAsString),
  fileTypes: parseAsArrayOf(parseAsAssetFileType),
  unitIds: parseAsArrayOf(parseAsInteger),
};

const urlKeys: UrlKeys<typeof paginatedPatchesSearchParams> = {
  assetFileHashes: "assetFileHash",
  fileTypes: "fileType",
  unitIds: "unitId",
};

export const loadPaginatedPatchesSearchParams = createLoader(
  paginatedPatchesSearchParams,
  {
    urlKeys: urlKeys,
  },
);
