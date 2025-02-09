// Mimic the hook returned by `create`
import {createContext, useContext} from 'react'
import { useStore } from 'zustand'
import { AppState, AppStore } from '../stores/app-store'

export const AppContext = createContext<AppStore | null>(null)

export function useAppContext<T>(selector: (state: AppState) => T): T {
  const store = useContext(AppContext)
  if (!store) throw new Error('Missing AppContext.Provider in the tree')
  return useStore(store, selector)
}