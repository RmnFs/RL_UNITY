
# Reinforcement Learning Survival Agent in Unity  

This repository contains the project files and implementation for **Reinforcement Learning (RL)** agent using the **Unity ML‑Agents Toolkit**.  
The project focuses on developing an RL agent capable of surviving in a progressively complex environment through **reward shaping** and **curriculum learning**.

---

##  Project Overview  

The aim of this project was to train an RL agent to survive in a custom Unity environment.  
The environment was incrementally extended over four levels, each introducing new challenges:

| Level | Primary Mechanics | Description |
|-------|--------------------|--------------|
| **1** | Energy collection | Collect energy pellets to survive. |
| **2** | Heat management | Movement and pellet collection generate heat; overheating ends episodes. |
| **3** | Cooling pellets | Collect cooling pellets to reduce heat strategically. |
| **4** | Dynamic obstacle | Avoid a moving obstacle while managing energy and heat. |

Reward design, stepwise environment tuning, and curriculum training were used to guide learning across these stages.

---

## Key Concepts  

- **Reinforcement Learning (RL)** — Agent learns optimal actions by receiving rewards and penalties through interaction.  
- **Reward Shaping** — Adjusting rewards to prevent pathological behaviors such as freezing, suicidal termination, or reckless overheating.  
- **Curriculum Learning** — Training agents on simpler environments before gradually increasing difficulty.  
- **Observation and Action Spaces** — Defining what the agent perceives, and how it moves or reacts within Unity.  

---

##  Tools and Frameworks  

- **Unity Game Engine** (2022.x+) — Environment simulation and physics.  
- **Unity ML‑Agents Toolkit** — RL interface connecting Unity with PyTorch.  
- **PyTorch Backend** — Policy training using PPO (Proximal Policy Optimization).  
- **C# Scripts** — Custom logic for energy, heat, cooling, and obstacles.  
- **TensorBoard** — Monitoring training through logs and reward curves.

---

| Level | Main Focus           | Training Steps     | Success Rate |
|-------|----------------------|--------------------|---------------|
| 1     | Energy management    | 500k               | ~95%          |
| 2     | Heat regulation      | 1M                 | ~85–90%       |
| 3     | Cooling pellet usage | 1M                 | ~90–92%       |
| 4     | Dynamic obstacle     | 6M (curriculum)    | ~88–90%       |
```

 Key Findings

1. Reward shaping critically influences learning stability and agent behavior.

2. Small imbalances in rewards can lead to freezing, reckless movement, or suicidal behavior.

3. Curriculum learning and longer training times significantly improve success in complex tasks.

4. Observation space expansion (adding obstacle velocity and position) improved, but did not fully solve, timing precision.

