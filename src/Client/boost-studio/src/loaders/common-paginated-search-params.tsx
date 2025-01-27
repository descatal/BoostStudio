import {
  createLoader,
  parseAsArrayOf,
  parseAsInteger,
  parseAsString,
} from "nuqs"
import { z } from "zod"

// not used for now
export const searchParamsSchema = z.object({
  page: z.number().default(1),
  perPage: z.number().default(10),
  search: z.array(z.string()),
})

// not sure how to integrate zod with this yet, monitor here:
// https://nuqs.47ng.com/docs/server-side
// https://github.com/47ng/nuqs/discussions/446
export const commonPaginatedSearchParams = {
  page: parseAsInteger.withDefault(1),
  perPage: parseAsInteger.withDefault(10),
  search: parseAsArrayOf(parseAsString).withDefault([]),
}

export const loadPaginatedCommonSearchParams = createLoader(
  commonPaginatedSearchParams
)
