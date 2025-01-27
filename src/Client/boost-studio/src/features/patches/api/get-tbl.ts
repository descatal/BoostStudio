import { GetApiTblByIdRequest, PatchFileVersion } from "@/api/exvs"
import { tblApi } from "@/features/api"
import { keepPreviousData, queryOptions, useQuery } from "@tanstack/react-query"

export const getTbl = (params: GetApiTblByIdRequest) => {
  return tblApi.getApiTblById(params)
}

export const getTblByIdQueryOptions = (id: PatchFileVersion) => {
  return queryOptions({
    queryKey: ["tbl-by-id", id],
    queryFn: () =>
      getTbl({
        id,
      }),
    placeholderData: keepPreviousData,
  })
}

export const useTblById = (id: PatchFileVersion) => {
  return useQuery({
    ...getTblByIdQueryOptions(id),
  })
}
