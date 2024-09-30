export interface SearchParams {
  [key: string]: string | string[] | undefined
}

export interface Option {
  label: string
  value: string
  icon?: React.ComponentType<{ className?: string }>
  withCount?: boolean
}

export interface DataTableInputFilterField<TData> {
  type: "input"
  label: string
  value: keyof TData
  placeholder: string
}

export interface DataTableSelectFilterField<TData> {
  type: "select"
  label: string
  value: keyof TData
  options: Option[]
}

export interface DataTableUnitFilterField<TData> {
  type: "unit"
  label: string
  value: keyof TData
}

export type DataTableFilterField<TData> =
  | DataTableInputFilterField<TData>
  | DataTableSelectFilterField<TData>
  | DataTableUnitFilterField<TData>

export interface DataTableFilterOption<TData> {
  id: string
  label: string
  value: keyof TData
  options: Option[]
  filterValues?: string[]
  filterOperator?: string
  isMulti?: boolean
}
