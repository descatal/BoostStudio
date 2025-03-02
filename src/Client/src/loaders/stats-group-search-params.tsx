import { commonPaginatedSearchParams } from "@/loaders/common-paginated-search-params";
import {
  createLoader,
  parseAsArrayOf,
  parseAsInteger,
  parseAsString,
  UrlKeys,
} from "nuqs";

const paginatedStatsGroupSearchParams = {
  ...commonPaginatedSearchParams,
  ids: parseAsArrayOf(parseAsString),
  unitIds: parseAsArrayOf(parseAsInteger),
};

const urlKeys: UrlKeys<typeof paginatedStatsGroupSearchParams> = {
  ids: "id",
  unitIds: "unitId",
};

export const loadPaginatedStatsGroupSearchParams = createLoader(
  paginatedStatsGroupSearchParams,
  {
    urlKeys: urlKeys,
  },
);
