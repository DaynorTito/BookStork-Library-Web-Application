import { SearchProvider } from "./context/SearchContext";
import AppRouter from "./router";
import "./App.css";


function App() {
  return (
    <SearchProvider>
      <AppRouter />
    </SearchProvider>
  );
}

export default App;
