import { commonPaginatedSearchParams } from "@/loaders/common-paginated-search-params";
import {
  createLoader,
  parseAsArrayOf,
  parseAsInteger,
  parseAsString,
  UrlKeys,
} from "nuqs";

const paginatedProjectilesSearchParams = {
  ...commonPaginatedSearchParams,
  hashes: parseAsArrayOf(parseAsString),
  unitIds: parseAsArrayOf(parseAsInteger),
  modelHashes: parseAsArrayOf(parseAsString),
};

const urlKeys: UrlKeys<typeof paginatedProjectilesSearchParams> = {
  hashes: "hash",
  unitIds: "unitId",
  modelHashes: "modelHash",
};

export const loadPaginatedProjectilesSearchParams = createLoader(
  paginatedProjectilesSearchParams,
  {
    urlKeys: urlKeys,
  },
);
