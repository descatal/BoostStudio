import {Input} from "@/components/ui/input";
import {
  ColumnDef, ColumnFiltersState,
  getCoreRowModel,
  getFacetedRowModel,
  getFacetedUniqueValues,
  getSortedRowModel, OnChangeFn, PaginationState, RowData, SortingState,
  Table,
  useReactTable,
  VisibilityState
} from "@tanstack/react-table";
import * as React from "react";
import _ from "lodash";
import {useEffect, useState} from "react";

declare module '@tanstack/react-table' {
  interface TableMeta<TData extends RowData> {
    modifiedRows: number[]
    fetchData: () => Promise<void>
    saveData: () => Promise<void>
    updateData: (rowIndex: number, columnId: string, value: unknown) => void
  }
}

declare module '@tanstack/react-table' {
  interface ColumnMeta<TData extends RowData, TValue> {
    isKey?: boolean | undefined
    isAction?: boolean | undefined
  }
}

interface DataTableProps<TData, TValue> {
  columns: ColumnDef<TData, TValue>[]
  data: TData[],
  setData: OnChangeFn<TData[]>,
  rowCount: number,
  fetchData: () => Promise<void> 
  saveData: () => Promise<void>
}

export default function useCustomReactTable<TData, TValue>(
  {
    columns,
    data,
    setData,
    rowCount,
    fetchData,
    saveData,
  }: DataTableProps<TData, TValue>
): Table<TData> {
  const [rowSelection, setRowSelection] = React.useState({})
  const [columnVisibility, setColumnVisibility] = React.useState<VisibilityState>({})
  const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]) // can set initial column filter state here
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  });
  const [sorting, setSorting] = useState<SortingState>([])
  const [modifiedRows, setModifiedRows] = useState<number[]>([])

  useEffect(() => {
    fetchData().catch()
  }, [pagination, sorting, columnFilters]);

  useEffect(() => {
    setPagination({ pageIndex: 0, pageSize: 10 })
  }, [columnFilters])
  
  // Give our default column cell renderer editing superpowers!
  const defaultColumn: Partial<ColumnDef<TData>> = {
    cell: ({getValue, row: {index}, column: {id}, table}) => {
      const initialValue = getValue()
      // We need to keep and update the state of the cell normally
      const [value, setValue] = React.useState(initialValue)

      // When the input is blurred, we'll call our table meta's updateData function
      const onBlur = () => {
        table.options.meta?.updateData(index, id, value)
      }

      // If the initialValue is changed external, sync it up with our state
      React.useEffect(() => {
        setValue(initialValue)
      }, [initialValue])

      return (
        <Input
          className={`text-center border-hidden w-full`}
          value={value as string}
          onChange={e => setValue(e.target.value)}
          onBlur={onBlur}
        />
      )
    },
  }

  return useReactTable({
    data: data,
    columns: columns,
    defaultColumn: defaultColumn,
    state: {
      sorting,
      columnVisibility,
      rowSelection,
      columnFilters,
      pagination
    },
    enableRowSelection: true,
    onRowSelectionChange: setRowSelection,
    onSortingChange: setSorting,
    onColumnFiltersChange: setColumnFilters,
    onColumnVisibilityChange: setColumnVisibility,
    getCoreRowModel: getCoreRowModel(),
    manualPagination: true,
    manualFiltering: true,
    rowCount: rowCount,
    onPaginationChange: setPagination, //update the pagination state when internal APIs mutate the pagination state
    getSortedRowModel:
      getSortedRowModel(),
    getFacetedRowModel:
      getFacetedRowModel(),
    getFacetedUniqueValues:
      getFacetedUniqueValues(),
    meta: {
      modifiedRows: modifiedRows,
      fetchData: async () => {
        setModifiedRows([])
        await fetchData()
      },
      saveData: async () => {
        setModifiedRows([])
        await saveData()
      },
      updateData: (rowIndex, columnId, value) => {
        setData(old =>
          old.map((row, index) => {
            if (index === rowIndex) {
              const oldValue = {...old[rowIndex]}
              const newValue = {...old[rowIndex], [columnId]: value}
              
              if (!_.isEqual(oldValue, newValue) && modifiedRows.indexOf(index) === -1) {
                modifiedRows.push(rowIndex)
                setModifiedRows(modifiedRows)
              }

              return {
                ...old[rowIndex]!,
                [columnId]: value,
              }
            }

            return row
          })
        )
      },
    },
  });
}
