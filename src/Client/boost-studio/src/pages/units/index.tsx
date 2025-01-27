import React, {useState} from "react"
import {UnitSummaryVm} from "@/api/exvs"
import {Link} from "react-router-dom"

import {Button} from "@/components/ui/button"
import {Input} from "@/components/ui/input"
import {Separator} from "@/components/ui/separator"

import UnitCard from "./components/unit-card"
import {useDebounce} from "@uidotdev/usehooks"
import {useSeriesUnits} from "@/features/series/api/get-series";
import TopBar from "@/components/custom/top-bar";

const UnitsPage = () => {
  // const unitsApi = useAppContext((s) => s.unitsApi)

  const [search, setSearch] = useState("")
  const debouncedSearchParam = useDebounce(search, 1000)
  const [selectedUnit, setSelectedUnit] = useState<UnitSummaryVm | undefined>(
    undefined
  )

  const seriesUnitsQuery = useSeriesUnits();
  const seriesUnits = seriesUnitsQuery.data?.items;

  // const query = useQuery({
  //     queryKey: ["getApiUnits", debouncedSearchParam],
  //     queryFn: async () => {
  //       const unitsSummary = await unitsApi.getApiUnits({
  //         search: debouncedSearchParam
  //       })
  //       return Object.groupBy(unitsSummary, (unitSummaryVm) => {
  //         return unitSummaryVm.series?.slugName ?? "unknown"
  //       })
  //     },
  //     placeholderData: keepPreviousData,
  //     staleTime: 1000
  //   }
  // );
  //
  // const groupedData = query.data;

  return (
    <div className="flex flex-col items-center">
      <TopBar className={"w-full justify-between"}>
        <Input
          placeholder={"Search units"}
          value={search}
          onChange={e => setSearch(e.target.value)}
          className={"m-2 h-8 w-[300px]"}
        />
        {selectedUnit && (
          <Link to={`/units/${selectedUnit.unitId}/customize/`}>
            <Button>Edit</Button>
          </Link>
        )}
      </TopBar>
      {seriesUnits && seriesUnits.map((vm) => (
        <div className={"flex flex-col items-center p-4"}>
          {vm.nameEnglish ?? "Unknown"}
          <div className={"grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 p-6"}>
            {
              vm.units && vm.units.map((unit) => (
                <UnitCard
                  className={"cursor-pointer"}
                  onClick={() => setSelectedUnit(unit)}
                  key={unit.unitId}
                  unit={unit}
                  selected={selectedUnit?.unitId === unit.unitId}
                />
              ))
            }
          </div>
          <Separator/>
        </div>
      ))}
      {/*{groupedData && Object.keys(groupedData)?.map((seriesSlugName) => (*/}
      {/*  <>*/}
      {/*    {groupedData[seriesSlugName] ? groupedData[seriesSlugName]![0].series?.nameEnglish ?? "Unknown" : "Unknown"}*/}
      {/*    <div className={"grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 p-6"}>*/}
      {/*      {*/}
      {/*        groupedData[seriesSlugName]?.map((unit) => (*/}
      {/*          <UnitCard*/}
      {/*            className={"cursor-pointer"}*/}
      {/*            onClick={() => setSelectedUnit(unit)}*/}
      {/*            key={unit.unitId}*/}
      {/*            unit={unit}*/}
      {/*            selected={selectedUnit?.unitId === unit.unitId}*/}
      {/*          />*/}
      {/*        ))*/}
      {/*      }*/}
      {/*    </div>*/}
      {/*    <Separator/>*/}
      {/*  </>*/}
      {/*))}*/}
    </div>
  )
}

export default UnitsPage
