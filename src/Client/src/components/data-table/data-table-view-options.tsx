"use client"

import {DropdownMenuTrigger} from "@radix-ui/react-dropdown-menu"
import {DotFilledIcon, DotIcon, MixerHorizontalIcon, ValueNoneIcon} from "@radix-ui/react-icons"
import {Table} from "@tanstack/react-table"

import {Button} from "@/components/ui/button"
import {
  DropdownMenu,
  DropdownMenuCheckboxItem,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuSeparator,
} from "@/components/ui/dropdown-menu"
import {Input} from "@/components/ui/input";
import {useEffect, useState} from "react";

interface DataTableViewOptionsProps<TData> {
  table: Table<TData>
}

export function DataTableViewOptions<TData>({
                                              table,
                                            }: DataTableViewOptionsProps<TData>) {
  const [search, setSearch] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("")

  useEffect(() => {
    const delayInputTimeoutId = setTimeout(() => {
      setDebouncedSearch(search);
    }, 200);
    return () => clearTimeout(delayInputTimeoutId);
  }, [search, 200]);

  const getAllFilteredColumns = table
    .getAllColumns()
    .filter((column) => column.id.toLowerCase().match(debouncedSearch.toLowerCase()))

  return (
    <DropdownMenu modal={false}>
      <DropdownMenuTrigger asChild>
        <Button
          variant="outline"
          size="sm"
          className="ml-auto h-8 lg:flex"
        >
          <MixerHorizontalIcon className="mr-2 h-4 w-4"/>
          View
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end" className={"overflow-y-scroll"}>
        <DropdownMenuLabel>Toggle columns</DropdownMenuLabel>
        <DropdownMenuSeparator/>
        <div onKeyDown={(e) => e.stopPropagation()}>
          <Input
            placeholder="Search..."
            value={search}
            onChange={
              (event) => {
                event.stopPropagation();
                event.preventDefault();
                setSearch(event.target.value)
              }
            }
            className="m-2 h-8 w-[300px]"
          />
        </div>
        <DropdownMenuSeparator/>
        <div className="my-2 flex flex-row gap-2 justify-around">
          <Button variant="outline"
                  size="sm"
                  className="h-8"
                  onClick={() => {
                    getAllFilteredColumns
                      .forEach((column) => column.toggleVisibility(true))
                  }}>
            <DotFilledIcon className="mr-2 h-4 w-4"/>
            Select All
          </Button>
          <Button variant="outline"
                  size="sm"
                  className="h-8"
                  onClick={() => {
                    getAllFilteredColumns
                      .filter(col => !col.columnDef.meta?.isKey)
                      .filter(col => !col.columnDef.meta?.isAction)
                      .forEach((column) => column.toggleVisibility(false))
                  }}>
            <DotIcon className="mr-2 h-4 w-4"/>
            Deselect All
          </Button>
        </div>
        <DropdownMenuSeparator/>
        <div>
          {getAllFilteredColumns
            .filter(
              (column) =>
                typeof column.accessorFn !== "undefined" && column.getCanHide()
            )
            .map((column) => {
              return (
                <DropdownMenuCheckboxItem
                  key={column.id}
                  className="capitalize"
                  checked={column.getIsVisible()}
                  onCheckedChange={(value) => column.toggleVisibility(value)}
                >
                  {column.id}
                </DropdownMenuCheckboxItem>
              )
            })}
        </div>
      </DropdownMenuContent>
    </DropdownMenu>
  )
}