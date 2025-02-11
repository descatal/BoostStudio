import { seriesApi } from "@/api/api"
import { queryOptions, useQuery } from "@tanstack/react-query"

export const getSeriesUnits = () => {
  return seriesApi.getApiSeriesUnits({
    listAll: true,
  })
}

export const getSeriesUnitsQueryOptions = () => {
  return queryOptions({
    queryKey: ["series-units"],
    queryFn: () => getSeriesUnits(),
  })
}

export const useSeriesUnits = () => {
  return useQuery({
    ...getSeriesUnitsQueryOptions(),
  })
}
