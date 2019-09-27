.thumb
push	{lr}
ldr	r0,=#0x3001160
mov	r1,#0x08
mov	r2,#0
ldr	r3,spawnEnemy
mov	lr,r3
.short	0xF800
ldr	r0,=#0x3001160
mov	r1,#0x08
mov	r2,#0
ldr	r3,spawnEnemy
mov	lr,r3
.short	0xF800
ldr	r0,=#0x3001160
mov	r1,#0x08
mov	r2,#0
ldr	r3,spawnEnemy
mov	lr,r3
.short	0xF800
pop	{pc}
.align
.ltorg
spawnEnemy:
