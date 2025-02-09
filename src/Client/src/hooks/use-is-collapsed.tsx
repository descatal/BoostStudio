import { useEffect } from "react"

import useLocalStorage from "./use-local-storage"

export default function useIsCollapsed() {
  const [isCollapsed, setIsCollapsed] = useLocalStorage({
    key: "collapsed-sidebar",
    defaultValue: false,
  })

  const [isMobileView, setIsMobileView] = useLocalStorage({
    key: "mobile-view",
    defaultValue: false,
  })

  useEffect(() => {
    const handleResize = () => {
      // Update isCollapsed based on window.innerWidth
      // mobile view
      if (window.innerWidth < 768) {
        setIsCollapsed(false)
        setIsMobileView(true)
      } else {
        // if previously was mobile but resized to normal, set the collapsed to true to make it not auto expand
        if (isMobileView) {
          setIsCollapsed(true)
        } else {
          setIsCollapsed(isCollapsed)
        }

        setIsMobileView(false)
      }
    }

    // Initial setup
    handleResize()

    // Add event listener for window resize
    window.addEventListener("resize", handleResize)

    // Cleanup event listener on component unmount
    return () => {
      window.removeEventListener("resize", handleResize)
    }
  }, [isCollapsed, setIsCollapsed, isMobileView, setIsMobileView])

  return [isCollapsed, setIsCollapsed] as const
}
