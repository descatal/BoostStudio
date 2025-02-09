import React from "react"
import type { UnitAmmoSlotDto } from "@/api/exvs"

export type AmmoSlotContextType = {
  ammoSlots: UnitAmmoSlotDto[]
  setAmmoSlots: (data: UnitAmmoSlotDto[]) => void
}

export const AmmoSlotContext = React.createContext<AmmoSlotContextType>({
  ammoSlots: [],
  setAmmoSlots: () => {},
})
