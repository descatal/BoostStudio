import * as React from "react"
import { CheckIcon } from "@radix-ui/react-icons"
import { useVirtualizer } from "@tanstack/react-virtual"
import { Check } from "lucide-react"

import { cn } from "@/lib/utils"
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
} from "@/components/ui/command"

import { Avatar, AvatarFallback, AvatarImage } from "./ui/avatar"

type VirtualizedCommandOption = {
  value: string
  label: string
  imageSrc?: string
}

interface VirtualizedCommandProps {
  height: string
  options: VirtualizedCommandOption[]
  placeholder: string
  selectedOptions: string[]
  onSelectOptions?: (options: string[]) => void
  multipleSelect?: boolean | undefined
}

const VirtualizedCommand = ({
  height,
  options,
  placeholder,
  selectedOptions,
  onSelectOptions,
  multipleSelect,
}: VirtualizedCommandProps) => {
  const [filteredOptions, setFilteredOptions] =
    React.useState<VirtualizedCommandOption[]>(options)

  const parentRef = React.useRef(null)

  const virtualizer = useVirtualizer({
    count: filteredOptions.length,
    getScrollElement: () => parentRef.current,
    estimateSize: () => 35,
    overscan: 5,
  })

  const virtualOptions = virtualizer.getVirtualItems()

  const handleSearch = (search: string) => {
    setFilteredOptions(
      options.filter((option) =>
        option.label.toLowerCase().includes(search.toLowerCase() ?? [])
      )
    )
  }

  const handleKeyDown = (event: React.KeyboardEvent) => {
    if (event.key === "ArrowDown" || event.key === "ArrowUp") {
      event.preventDefault()
    }
  }

  return (
    <Command shouldFilter={false} onKeyDown={handleKeyDown}>
      <CommandInput onValueChange={handleSearch} placeholder={placeholder} />
      <CommandEmpty>No item found.</CommandEmpty>
      <CommandGroup
        ref={parentRef}
        style={{
          height: height,
          width: "100%",
          overflow: "auto",
        }}
      >
        <div
          style={{
            height: `${virtualizer.getTotalSize()}px`,
            width: "100%",
            position: "relative",
          }}
        >
          {virtualOptions.map((virtualOption) => (
            <CommandItem
              style={{
                position: "absolute",
                top: 0,
                left: 0,
                width: "100%",
                height: `${virtualOption.size}px`,
                transform: `translateY(${virtualOption.start}px)`,
              }}
              key={filteredOptions[virtualOption.index].value}
              value={filteredOptions[virtualOption.index].value}
              onSelect={() => {
                if (!onSelectOptions) return

                if (!selectedOptions) selectedOptions = []
                const selectedOption =
                  filteredOptions[virtualOption.index].value

                if (multipleSelect) {
                  // add the newly selected unit to the list
                  // if it is previously selected, remove it
                  const ifExist = selectedOptions.some(
                    (option) => option === selectedOption
                  )

                  if (ifExist) {
                    // deselect the unit
                    const filteredUnits = selectedOptions.filter(
                      (selectOption) => selectOption !== selectedOption
                    )
                    onSelectOptions(filteredUnits)
                  } else {
                    onSelectOptions([...selectedOptions, selectedOption])
                  }
                } else {
                  onSelectOptions([selectedOption])
                }
              }}
            >
              {filteredOptions[virtualOption.index].imageSrc && (
                <Avatar className="mr-2 h-5 w-5">
                  <AvatarImage
                    src={filteredOptions[virtualOption.index].imageSrc}
                    alt={filteredOptions[virtualOption.index].label}
                  />
                  <AvatarFallback>
                    {filteredOptions[virtualOption.index].label.charAt(0)}
                  </AvatarFallback>
                </Avatar>
              )}
              {filteredOptions[virtualOption.index].label}
              <Check
                className={cn(
                  "ml-auto h-4 w-4",
                  selectedOptions?.some(
                    (selectedOption) =>
                      selectedOption ===
                      filteredOptions[virtualOption.index].value
                  )
                    ? "opacity-100"
                    : "opacity-0"
                )}
              />
            </CommandItem>
          ))}
        </div>
      </CommandGroup>
    </Command>
  )
}

export default VirtualizedCommand
