import { commonPaginatedSearchParams } from "@/loaders/common-paginated-search-params";
import {
  createLoader,
  parseAsArrayOf,
  parseAsInteger,
  parseAsString,
  UrlKeys,
} from "nuqs";

const paginatedHitboxesSearchParams = {
  ...commonPaginatedSearchParams,
  hashes: parseAsArrayOf(parseAsString),
  unitIds: parseAsArrayOf(parseAsInteger),
};

const urlKeys: UrlKeys<typeof paginatedHitboxesSearchParams> = {
  hashes: "hash",
  unitIds: "unitId",
};

export const loadPaginatedHitboxesSearchParams = createLoader(
  paginatedHitboxesSearchParams,
  {
    urlKeys: urlKeys,
  },
);
