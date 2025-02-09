import { AssetFileType } from "@/api/exvs"
import { commonPaginatedSearchParams } from "@/loaders/common-paginated-search-params"
import {
  createLoader,
  createParser,
  parseAsArrayOf,
  parseAsInteger,
  UrlKeys,
} from "nuqs"

const parseAsAssetFileType = createParser({
  parse(queryValue) {
    if (Object.values(AssetFileType).includes(queryValue as AssetFileType)) {
      return queryValue as AssetFileType
    }
    return AssetFileType.Unknown
  },
  serialize(value) {
    return value
  },
})

const paginatedPatchesSearchParams = {
  ...commonPaginatedSearchParams,
  assetFileHashes: parseAsArrayOf(parseAsInteger),
  fileTypes: parseAsArrayOf(parseAsAssetFileType),
}

export const coordinatesUrlKeys: UrlKeys<typeof paginatedPatchesSearchParams> =
  {
    assetFileHashes: "assetFileHash",
    fileTypes: "fileType",
  }

export const loadPaginatedPatchesSearchParams = createLoader(
  paginatedPatchesSearchParams,
  {
    urlKeys: coordinatesUrlKeys,
  }
)
