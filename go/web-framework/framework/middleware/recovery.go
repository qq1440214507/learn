package middleware

import "web-framework/framework"

func Recovery() framework.ControllerHandler {
	return func(c *framework.Context) error {
		defer func() {
			if err := recover(); err != nil {
				c.Json(map[string]interface{}{
					"message": "error",
				})
			}
		}()
		c.Next()
		return nil
	}
}
