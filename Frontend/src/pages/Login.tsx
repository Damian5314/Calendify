import React from "react";

const Login: React.FC = () => {

  return (
    <div style={{ fontFamily: "Arial, sans-serif", padding: "20px", maxWidth: "400px", margin: "50px auto", border: "1px solid #ccc", borderRadius: "10px" }}>
      <h2 style={{ textAlign: "center" }}>Sign In</h2>
      <form>
        {/* Email Input */}
        <div style={{ marginBottom: "20px" }}>
          <label htmlFor="email" style={{ display: "block", marginBottom: "5px" }}>Email *</label>
          <input
            type="email"
            id="email"
            style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
            required
          />
        </div>

        {/* Password Input */}
        <div style={{ marginBottom: "20px" }}>
          <label htmlFor="password" style={{ display: "block", marginBottom: "5px" }}>Password *</label>
          <input
            type="password"
            id="password"
            style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
            required
          />
        </div>

        {/* Remember Me and Help */}
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "20px" }}>
          <label>
            <input type="checkbox" style={{ marginRight: "5px" }} />
            Remember me
          </label>
          <a href="#" style={{ textDecoration: "none", color: "#007BFF" }}>Password Help?</a>
        </div>

        {/* Sign In Button */}
        <button
          type="submit"
          style={{
            width: "100%",
            padding: "10px",
            backgroundColor: "black",
            color: "white",
            border: "none",
            borderRadius: "5px",
            fontSize: "16px",
            cursor: "pointer",
          }}
        >
          Sign In
        </button>
      </form>

      {/* Register Link */}
      <div style={{ marginTop: "20px", textAlign: "center" }}>
        <p>
          Don’t have an account?{" "}
          <a href="#" style={{ textDecoration: "none", color: "#007BFF" }}>Create One Now</a>
        </p>
      </div>
    </div>
  );
};

export default Login;
