import { commonPaginatedSearchParams } from "@/loaders/common-paginated-search-params";
import {
  createLoader,
  parseAsArrayOf,
  parseAsInteger,
  parseAsString,
  UrlKeys,
} from "nuqs";

const paginatedAmmoSearchParams = {
  ...commonPaginatedSearchParams,
  hashes: parseAsArrayOf(parseAsString),
  unitIds: parseAsArrayOf(parseAsInteger),
};

const urlKeys: UrlKeys<typeof paginatedAmmoSearchParams> = {
  hashes: "hash",
  unitIds: "unitId",
};

export const loadPaginatedAmmoSearchParams = createLoader(
  paginatedAmmoSearchParams,
  {
    urlKeys: urlKeys,
  },
);
