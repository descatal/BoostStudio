import * as React from "react"
import { useEffect } from "react"
import { SiHexo } from "react-icons/si"

import { cn } from "@/lib/utils"
import { Label } from "@/components/ui/label"
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover"
import { ToggleGroup, ToggleGroupItem } from "@/components/ui/toggle-group"
import { CopyButton } from "@/components/custom/copy-button"
import { IconInput } from "@/components/custom/icon-input"

import { HoverCard, HoverCardContent, HoverCardTrigger } from "../ui/hover-card"

export interface InputProps
  extends React.InputHTMLAttributes<HTMLInputElement> {
  initialMode?: "hex" | "dec"
  initialValue?: number | undefined | null
  readonly?: boolean
  onHashChanged?: (value: number | undefined) => void
}

const HashInput = React.forwardRef<HTMLInputElement, InputProps>(
  (
    {
      className,
      type,
      initialValue,
      initialMode,
      readonly,
      onHashChanged,
      ...props
    },
    ref
  ) => {
    const [mode, setMode] = React.useState<"hex" | "dec">("hex")
    const [parsedValue, setParsedValue] = React.useState<number | undefined>()

    const hexadecimalValue = parsedValue?.toString(16)?.toUpperCase() ?? ""
    const decimalValue = parsedValue?.toString() ?? ""

    function handleInputChange(value: string | undefined) {
      const parsedValue = value
        ? parseInt(value as string, mode === "hex" ? 16 : 10)
        : undefined
      setParsedValue(parsedValue)
    }

    // still a bug where if the value did not change it will not update
    useEffect(() => {
      // passed in props value must be parsed as decimal number first
      handleInputChange(initialValue?.toString(mode === "hex" ? 16 : 10))
    }, [initialValue])

    useEffect(() => {
      if (onHashChanged) onHashChanged(parsedValue)
    }, [parsedValue])

    useEffect(() => {
      setMode(initialMode ?? "hex")
    }, [])

    return (
      <Popover>
        <PopoverTrigger asChild>
          <div className="relative flex w-full items-center space-x-1">
            <IconInput
              type={type}
              className={cn(className)}
              ref={ref}
              autoComplete={"off"}
              readonly={readonly}
              {...props}
              value={mode === "hex" ? hexadecimalValue : decimalValue}
              onChange={(e) => {
                handleInputChange(e.target.value)
              }}
              startIcon={SiHexo}
            />
          </div>
        </PopoverTrigger>
        <PopoverContent onOpenAutoFocus={(e) => e.preventDefault()}>
          <div className={`space-y-4`}>
            <div className={"flex items-center justify-between space-x-2"}>
              <Label className={"text-xs font-light"}>Input Mode</Label>
              <ToggleGroup
                value={mode}
                onValueChange={(e) => {
                  setMode(e as "hex" | "dec")
                }}
                type="single"
                defaultValue="h"
                variant="outline"
              >
                <ToggleGroupItem value="hex">Hex</ToggleGroupItem>
                <ToggleGroupItem value="dec">Dec</ToggleGroupItem>
              </ToggleGroup>
            </div>
            <div className="flex items-center justify-between space-x-4">
              <div className={"flex flex-col space-y-2"}>
                <Label className={"text-xs font-light"}>Hexadecimal</Label>
                <Label className="text-lg font-semibold">
                  {hexadecimalValue}
                </Label>
              </div>
              <CopyButton type={"button"} value={hexadecimalValue!} />
            </div>
            <div className="flex items-center justify-between space-x-4">
              <div className={"flex flex-col space-y-2"}>
                <Label className={"text-xs font-light"}>Decimal</Label>
                <Label className="text-lg font-semibold">{decimalValue}</Label>
              </div>
              <CopyButton type={"button"} value={decimalValue!} />
            </div>
          </div>
        </PopoverContent>
      </Popover>
    )
  }
)
HashInput.displayName = "Input"

export { HashInput }
