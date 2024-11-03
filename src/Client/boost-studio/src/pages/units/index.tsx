import React, { useCallback, useEffect, useState } from "react"
import { UnitDto } from "@/api/exvs"
import { fetchUnits } from "@/api/wrapper/units-api"
import { Link, useLocation } from "react-router-dom"

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Separator } from "@/components/ui/separator"

import UnitCard from "./components/unit-card"

const UnitsPage = () => {
  const [search, setSearch] = useState("")
  const [debouncedSearch, setDebouncedSearch] = useState("")

  const [units, setUnits] = useState<UnitDto[]>([])
  const [selectedUnit, setSelectedUnit] = useState<UnitDto | undefined>(
    undefined
  )

  const getData = useCallback(async () => {
    let units = await fetchUnits({
      search: debouncedSearch,
    })
    units = units.sort((a, b) => (a.unitId ?? 0) - (b.unitId ?? 0))
    setUnits(units)
  }, [debouncedSearch])

  useEffect(() => {
    getData().catch((e) => console.error(e))
  }, [])

  useEffect(() => {
    getData().catch((e) => console.error(e))
  }, [debouncedSearch])

  useEffect(() => {
    const delayInputTimeoutId = setTimeout(() => {
      setDebouncedSearch(search)
    }, 200)
    return () => clearTimeout(delayInputTimeoutId)
  }, [search, 200])

  return (
    <div className="flex flex-col items-center">
      <div className="space-x-42 flex w-full items-center justify-between p-6">
        <Input
          placeholder={"Search units"}
          value={search}
          onChange={(event) => {
            setSearch(event.target.value)
          }}
          className={"m-2 h-8 w-[300px]"}
        />
        {selectedUnit && (
          <Link to={`/units/${selectedUnit.unitId}/customize/`}>
            <Button>Edit</Button>
          </Link>
        )}
      </div>
      <Separator />
      <div className="grid gap-3 p-6 md:grid-cols-2 lg:grid-cols-3">
        {units?.map((unit) => (
          <UnitCard
            className={"cursor-pointer"}
            onClick={() => setSelectedUnit(unit)}
            key={unit.unitId}
            unit={unit}
            selected={selectedUnit?.unitId === unit.unitId}
          />
        ))}
      </div>
    </div>
  )
}

export default UnitsPage
