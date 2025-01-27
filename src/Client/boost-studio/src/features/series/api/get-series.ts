import {queryOptions, useQuery} from "@tanstack/react-query";
import {Configuration, SeriesApi} from "@/api/exvs";

const api = new SeriesApi(new Configuration({
  basePath: "https://localhost:5001"
}));

export const getSeriesUnits = () => {
  return api.getApiSeriesUnits({
    listAll: true,
  });
};

export const getSeriesUnitsQueryOptions = () => {
  return queryOptions({
    queryKey: ['series-units'],
    queryFn: () => getSeriesUnits(),
  });
};

export const useSeriesUnits = () => {
  return useQuery({
    ...getSeriesUnitsQueryOptions(),
  });
};