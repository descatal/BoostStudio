import React, {useCallback, useEffect, useState} from 'react';
import {fetchUnits} from "@/api/wrapper/units-api";
import { UnitDto } from '@/api/exvs';
import UnitCard from './unit-card';
import {Input} from "@/components/ui/input";

const UnitsPage = () => {
  const [search, setSearch] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("")
  const [units, setUnits] = useState<UnitDto[]>([]);

  const getData = useCallback( async () => {
    let units = await fetchUnits({
      search: debouncedSearch,
    })
    units = units.sort((a, b) => (a.unitId ?? 0) - (b.unitId ?? 0))
    setUnits(units)
  }, [debouncedSearch]);

  useEffect(() => {
    getData().catch(console.error)
  }, []);

  useEffect(() => {
    getData().catch(console.error)
  }, [debouncedSearch]);
  
  useEffect(() => {
    const delayInputTimeoutId = setTimeout(() => {
      setDebouncedSearch(search);
    }, 200);
    return () => clearTimeout(delayInputTimeoutId);
  }, [search, 200]);
  
  return (
    <div className="flex flex-col items-center">
      <div className='p-6 ml-auto flex items-center space-x-4'>
        <Input
          placeholder={"Search units"}
          value={search}
          onChange={
            (event) => {
              setSearch(event.target.value)
            }
          }
          className={"m-2 h-8 w-[300px]"}
        />
      </div>
      <div className="p-6 grid gap-3 md:grid-cols-2 lg:grid-cols-3">
        {
          units?.map((unit, index) =>
            <UnitCard key={unit?.unitId} unit={unit}/>
          )
        }
      </div>
    </div>
  );
};

export default UnitsPage;