.thumb
push	{lr}
ldr	r0,=#0x3001160
mov	r1,#0x34
mov	r2,#1
ldr	r3,spawnEnemy
mov	lr,r3
.short	0xF800
pop	{pc}
.align
.ltorg
spawnEnemy:
