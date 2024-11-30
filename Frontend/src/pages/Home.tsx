import React from "react";

// Helper function to get the start of the week (Monday)
const getStartOfWeek = (date: Date): Date => {
  const day = date.getDay();
  const diff = (day === 0 ? -6 : 1) - day; // Adjust for Monday start
  const startOfWeek = new Date(date);
  startOfWeek.setDate(date.getDate() + diff);
  return startOfWeek;
};

// Home component
const Home: React.FC = () => {
  const today = new Date();
  const startOfWeek = getStartOfWeek(today);
  const daysOfWeek = Array.from({ length: 7 }, (_, i) => {
    const day = new Date(startOfWeek);
    day.setDate(startOfWeek.getDate() + i);
    return day;
  });

  return (
    <div style={{ fontFamily: "Arial, sans-serif", padding: "20px" }}>
      {/* Header */}
      <div
        style={{
          backgroundColor: "#007BFF",
          color: "white",
          padding: "10px",
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        <div style={{ fontWeight: "bold", fontSize: "24px" }}>LOGO</div>
        <button
          style={{
            background: "none",
            border: "none",
            color: "white",
            fontSize: "16px",
            cursor: "pointer",
          }}
        >
          Register / Login
        </button>
      </div>

      {/* Greeting and Date Navigation */}
      <div
        style={{
          padding: "10px",
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        <div style={{ fontSize: "20px" }}>Hallo Gebruiker 3</div>
        <div style={{ display: "flex", alignItems: "center", gap: "10px" }}>
          <button style={{ fontSize: "20px" }}>&lt;</button>
          <div style={{ fontSize: "16px" }}>Vandaag</div>
          <button style={{ fontSize: "20px" }}>&gt;</button>
        </div>
        <div style={{ fontSize: "20px" }}>{today.toLocaleDateString()}</div>
        <div style={{ display: "flex", gap: "10px" }}>
          <button>Week</button>
          <button>Maand</button>
          <button>Jaar</button>
        </div>
        <button style={{ fontSize: "20px", padding: "5px" }}>+</button>
      </div>

      {/* Calendar Table */}
      <div
        style={{
          display: "grid",
          gridTemplateColumns: "repeat(7, 1fr)",
          borderTop: "2px solid black",
          borderBottom: "2px solid black",
        }}
      >
        {daysOfWeek.map((day, index) => (
          <div
            key={index}
            style={{
              border: "1px solid black",
              padding: "10px",
              textAlign: "center",
              fontWeight: "bold",
              backgroundColor:
                day.toDateString() === today.toDateString() ? "#FFFF99" : "white", // Highlight today
            }}
          >
            {["ma", "di", "wo", "do", "vr", "za", "zo"][index]}
            <div>{day.toLocaleDateString("nl-NL")}</div>
          </div>
        ))}
      </div>

      {/* Calendar Events */}
      <div style={{ display: "grid", gridTemplateColumns: "repeat(7, 1fr)" }}>
        <div style={{ border: "1px solid black", padding: "10px" }}>
          <div
            style={{
              border: "1px solid gray",
              padding: "5px",
              margin: "5px",
              backgroundColor: "#FFFAF0",
            }}
          >
            10:00 - 12:00
            <br />
            meeting
          </div>
        </div>
        {Array(6)
          .fill(null)
          .map((_, index) => (
            <div key={index} style={{ border: "1px solid black", padding: "10px" }}></div>
          ))}
      </div>
    </div>
  );
};

export default Home;
