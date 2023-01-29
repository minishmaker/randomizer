.thumb
push	{r4,lr}
mov	r4,r0
ldr	r2,table
loop:
ldrh	r1,[r2,#6]
cmp	r1,r4
beq	match
cmp	r1,#0
beq	vanilla
add	r2,#8
b	loop

match:
ldr	r0,[r2]
ldrh	r1,[r2,#4]
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	vanilla

mov	r0,#0
pop	{r4,pc}

vanilla:
mov	r0,r4
pop	{r4}
mov	r1,r0
sub	r0,r1,#1
cmp	r0,#0x63
bhi	goto801E844

ldr	r3,=#0x801E837
bx	r3

goto801E844:
ldr	r3,=#0x801E845
bx	r3
.align
.ltorg
table:
