"use client"

import * as React from "react"
import {flexRender, Table as TableType,} from "@tanstack/react-table"

import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow,} from "@/components/ui/table"
import {cn} from "@/lib/utils"
import { DataTablePagination } from "./data-table-pagination"


interface DataTableProps<TData> extends React.HTMLAttributes<HTMLDivElement> {
  table: TableType<TData>
}

export function DataTable<TData>({
                                   table,
                                   children,
                                   className,
                                   ...props
                                 }: DataTableProps<TData>) {
  const modifiedRows = table.options.meta?.modifiedRows ?? []

  return (
    <div
      className={cn("w-full space-y-2.5 overflow-auto", className)}
      {...props}
    >
      {children}
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            {table.getHeaderGroups().map((headerGroup) => (
              <TableRow key={headerGroup.id}>
                {headerGroup.headers.map((header) => {
                  return (
                    <TableHead
                      className={`
                        text-center
                        ${header.column.columnDef.meta?.isKey ? "sticky left-0 bg-background" : ""}
                        ${header.column.columnDef.meta?.isAction ? "sticky right-0 bg-background" : ""}`
                      }
                      key={header.id}
                      colSpan={header.colSpan}
                    >
                      {header.isPlaceholder
                        ? null
                        : flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                    </TableHead>
                  )
                })}
              </TableRow>
            ))}
          </TableHeader>
          <TableBody>
            {table.getRowModel().rows?.length ? (
              table.getRowModel().rows.map((row) => (
                <TableRow
                  key={row.id}
                  data-state={row.getIsSelected() && "selected"}
                >
                  {row.getVisibleCells().map((cell) => (
                    <TableCell
                      className={`
                        text-center
                        min-w-[200px]
                        bg-background
                        ${!(modifiedRows.indexOf(row.index) === -1)
                        ? cell.column.columnDef.meta?.isKey || cell.column.columnDef.meta?.isAction
                          ? cell.column.columnDef.meta?.isAction
                            ? "border-r-2 border-y-2 border-red-500"
                            : cell.column.columnDef.meta?.isKey
                              ? "border-l-2 border-y-2 border-red-500"
                              : "border-y-2 border-red-500"
                          : "border-y-2 border-red-500"
                        : ""}
                        ${cell.column.columnDef.meta?.isKey ? "sticky left-0" : ""}
                        ${cell.column.columnDef.meta?.isAction ? "sticky right-0" : ""}`
                      }
                      key={cell.id}
                    >
                      {flexRender(
                        cell.column.columnDef.cell,
                        cell.getContext()
                      )}
                    </TableCell>
                  ))}
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell
                  colSpan={table.getAllColumns().length}
                  className="h-24 text-center"
                >
                  No results.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>
      <div className="flex flex-col gap-2.5">
        <DataTablePagination table={table}/>
        {table.getFilteredSelectedRowModel().rows.length > 0}
      </div>
    </div>
  )
}