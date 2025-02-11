import { tblApi } from "@/api/api"
import {
  GetApiPatchFilesRequest,
  GetApiPatchFilesSummaryRequest,
} from "@/api/exvs"
import { keepPreviousData, queryOptions, useQuery } from "@tanstack/react-query"

export const getTblPatches = (params: GetApiPatchFilesSummaryRequest) => {
  return tblApi.getApiPatchFilesSummary(params)
}

export const getTblPatchFilesQueryOptions = (
  options: GetApiPatchFilesSummaryRequest
) => {
  return queryOptions({
    queryKey: ["tbl-patch-files-summary", options],
    queryFn: () =>
      getTblPatches({
        ...options,
      }),
    placeholderData: keepPreviousData,
  })
}

export const useTblPatchFiles = (options: GetApiPatchFilesSummaryRequest) => {
  return useQuery({
    ...getTblPatchFilesQueryOptions(options),
  })
}
