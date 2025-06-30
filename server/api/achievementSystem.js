const MessageHandler = require('./messageHandler.js');

class AchievementSystem extends MessageHandler {
    constructor(io, databaseConnector) {
        super(io, databaseConnector, {});
        this.achievementDefinitions = new Map();
        this.loadAchievements();
    }

    async loadAchievements() {
        try {
            const result = await this._databaseConnector.executePreparedQuery(
                "SELECT * FROM achievements WHERE is_active = TRUE"
            );
            
            result.rows.forEach(achievement => {
                this.achievementDefinitions.set(achievement.achievement_key, achievement);
            });
            
            console.log(`Loaded ${this.achievementDefinitions.size} achievements`);
        } catch (error) {
            console.error("Failed to load achievements:", error);
        }
    }

    async checkPieceAchievements(gameId, playerName, pieceId) {
        try {
            // Get current unit count for the player in this game
            const unitCount = await this.getPlayerUnitCount(gameId, playerName, pieceId);
            
            // Check unit-related achievements
            if (pieceId === 3) { // Unit piece
                await this.checkAchievement(gameId, playerName, 'unit_commander', unitCount, 10);
            } else if (pieceId === 1) { // ResourceCollector
                await this.checkAchievement(gameId, playerName, 'master_builder', unitCount, 5);
            } else if (pieceId === 2) { // DefensiveBuilding
                await this.checkAchievement(gameId, playerName, 'fortress_architect', unitCount, 3);
            }
        } catch (error) {
            console.error("Achievement check failed:", error);
        }
    }

    async getPlayerUnitCount(gameId, playerName, pieceId) {
        const result = await this._databaseConnector.executePreparedQuery(
            `SELECT COALESCE(placement_count, 0) as count 
                FROM player_piece_stats 
                WHERE game_id = ? AND player_name = ? AND piece_id = ?`,
            [gameId, playerName, pieceId]
        );
        
        return result.rows.length > 0 ? result.rows[0].count : 0;
    }

    async checkAchievement(gameId, playerName, achievementKey, currentProgress, targetProgress) {
        const achievement = this.achievementDefinitions.get(achievementKey);
        if (!achievement) return;

        try {
            console.log(`[ACH] Upserting achievement ${achievementKey} for game ${gameId}, player ${playerName}: current=${currentProgress}, target=${targetProgress}`);
            // Update or insert achievement progress
            await this._databaseConnector.executePreparedQuery(
                `INSERT INTO player_achievements 
                    (game_id, player_name, achievement_id, progress_current, progress_target, is_completed, completed_at)
                    VALUES (?, ?, ?, ?, ?, ?, ?)
                    ON DUPLICATE KEY UPDATE
                    progress_current = VALUES(progress_current),
                    is_completed = VALUES(is_completed),
                    completed_at = VALUES(completed_at)`,
                [
                    gameId,
                    playerName,
                    achievement.achievement_id,
                    currentProgress,
                    targetProgress,
                    currentProgress >= targetProgress,
                    currentProgress >= targetProgress ? new Date() : null
                ]
            );

            // If achievement just completed, notify the player
            if (currentProgress >= targetProgress) {
                await this.notifyAchievementUnlocked(gameId, playerName, achievement);
            }
        } catch (error) {
            console.error(`Failed to update achievement ${achievementKey}:`, error);
        }
    }

    async notifyAchievementUnlocked(gameId, playerName, achievement) {
        // Find the room this game belongs to
        const gameResult = await this._databaseConnector.executePreparedQuery(
            "SELECT room_id FROM games WHERE game_id = ?",
            [gameId]
        );
        
        if (gameResult.rows.length === 0) return;
        
        const roomId = gameResult.rows[0].room_id;
        
        // Emit achievement notification to the specific player
        this._io.to(roomId).emit("achievement unlocked", {
            playerName: playerName,
            achievement: {
                id: achievement.achievement_id,
                key: achievement.achievement_key,
                name: achievement.name,
                description: achievement.description,
                points: achievement.points,
                category: achievement.category
            }
        });

        console.log(`🏆 Achievement unlocked: ${playerName} earned "${achievement.name}"`);
        console.log(`📣 Emitted "achievement unlocked" to room ${roomId} for player ${playerName}`);

    }
}

module.exports = AchievementSystem;
