import { useEffect, useState } from 'react'
import './App.css'

function App() {
  const [data, setData] = useState("")

  const handleChange = (event) => {
    setData(event.target.value)
  }


  return (
    <>
      <form>
        <label>
          Name:
          <input type="text" value={data} onChange={handleChange} />
        </label>
      </form>
      <p>{data}</p>
    </>
  )
}

export default App
 /*
function Welcome({name, fullName, age}) {
  return <h1>Hello, {props.name}</h1>;
}

const Aoo = () => (
  <>
  <Welcome name="Sara" fullName={"Oeoe"} age={23} />
  <Welcome name="Cahal" fullName={"Cahal"} age={25} />
  </>
)
*/