.equ table, offset+4
.equ time, table+4
.thumb
ldr	r0,[r1,#0x50]
mov	r7,#1
neg	r7,r7
cmp	r0,r7
beq	noIncrease
add	r0,#1
str	r0,[r1,#0x50]
noIncrease:
ldrb	r0,[r4]
push	{r0-r7}
@if the bloom flag is set, increase bloom kill counter
ldr	r0,=#0x2002CA6
ldrb	r0,[r0]
mov	r1,#0x10
and	r0,r1
cmp	r0,#0
beq	nobloom
ldr	r0,=#0x2002ED0
ldr	r1,[r0]
ldr	r2,=#0xFFFFFFFF
cmp	r1,r2
beq	nobloom
add	r1,#1
str	r1,[r0]
nobloom:
ldrb	r0,[r5,#9]
cmp	r0,#0x50
bhi	end
ldr	r1,table
ldrb	r5,[r1,r0]
cmp	r5,#0xFF
beq	end
ldr	r2,offset
lsl	r1,r5,#1
ldrh	r0,[r2,r1]
cmp	r0,#0
bne	notFirst
push	{r0-r3}
ldr	r0,time
lsl	r1,r5,#2
ldr	r2,=#0x203FFF0
ldr	r2,[r2]
str	r2,[r0,r1]
pop	{r0-r3}
notFirst:
ldr	r3,=#0xFFFF
cmp	r0,r3
beq	end
add	r0,#1
strh	r0,[r2,r1]

end:
pop	{r0-r7}
ldr	r7,=#0x804A5A5
bx	r7
.align
.ltorg
offset:
